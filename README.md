# HH.YiDASDK — .NET 10

宜搭 API SDK，保留 `Req`、`Resp`、`Core`、`Util`、`YiDA` 的组织方式及泛型请求／响应模型。

- 目标框架 `net10.0`，使用 SDK 默认的稳定 C# 版本。
- HTTP 使用 `HttpClient` / `IHttpClientFactory`，JSON 使用 `System.Text.Json`。
- NuGet 依赖仅为微软组件：`Microsoft.Extensions.Http`、`Microsoft.Extensions.Options.ConfigurationExtensions`、`System.Threading.RateLimiting`，统一到 `10.0.0`。
- 支持多个应用与表单别名、配置启动校验、异步限流、业务重试、取消和流式附件下载。

[宜搭接口文档](https://www.yuque.com/yida/support/agb8im)

## 在当前解决方案中启用

SDK 已加入 `HH.Integration.slnx`，由 Infrastructure 引用，API 在组合根注册。API 会读取其内容根目录下可选的 `appsettings.YiDA.json`，也支持标准配置来源中的 `YiDAConfig` 节点。

首次配置时，在 API 项目目录中复制模板，然后填写实际配置：

```powershell
Copy-Item src/HH.Integration.Api/appsettings.YiDA.example.json src/HH.Integration.Api/appsettings.YiDA.json
```

当前工作目录已将 `plugins/HH.YiDASDK/appsettings.YiDA.json` 中的本机配置复制到 API 目录，后续运行配置统一维护在 `src/HH.Integration.Api/appsettings.YiDA.json`。两处实际配置文件都由 `.gitignore` 排除。SDK 不会自行读取插件源码目录中的配置文件。

API 项目会在构建和发布时复制实际的 `appsettings.YiDA.json`；空白模板只保留在源码中。运行时的配置节点仍然是 `YiDAConfig`，独立文件与其他配置来源合并到同一个 `IConfiguration`，无需在 `appsettings.json` 中重复填写。

API 中的 SDK 文件优先级低于标准 `appsettings*.json`、User Secrets、环境变量和命令行。可使用 `YiDAConfig__Key`、`YiDAConfig__Secret` 等环境变量覆盖。当前 API 接收实例 ID 后会通过 SDK 查询详情并下载附件，因此 `YiDAConfig` 为必需配置，缺少或无效时启动失败。业务默认使用 `yida:project` 表单映射及 `attachmentField_fnf9o9jjl` 附件字段，保存位置通过 `YiDaAttachments` 配置，详见[主项目说明](../../README.md#附件查询与保存)。

## 在其他宿主中注册

宿主负责加载配置文件和决定配置来源优先级，然后传入配置节：

```csharp
using HH.YiDASDK.Core;

builder.Services.AddYiDASdk(
    builder.Configuration.GetSection(YiDAConfig.SectionName));
```

也可使用配置委托：

```csharp
builder.Services.AddYiDASdk(options =>
{
    options.Key = "你的开发者 Key";
    options.Secret = "你的开发者 Secret";
    options.Nonce = "你的网关签名 Nonce";
    // BaseUrl、Version、ClientConfig、RequestMap 按实际应用配置。
});
```

`Key`、`Secret`、`Nonce`、`Version` 必须非空，`BaseUrl` 必须为有效 HTTP(S) 地址。`RequestMap` 可以为空，此时需手动构造请求并设置应用凭据。配置了应用模板时，其 `appType`、`systemToken`、`userId` 以及已声明的表单映射必须有效。

配置结构见 [appsettings.YiDA.example.json](../../src/HH.Integration.Api/appsettings.YiDA.example.json)。沿用 `YiDAConfig`、`ClientConfig`、`RequestMap`、`Request`、`FormUuidMap`，配置绑定不区分大小写。

`IYiDAClient` 和直接注入的 `YiDAConfig` 均为 Scoped，使用同一份 `IOptionsSnapshot<YiDAConfig>`。配置重载后，新作用域读取更新值，当前作用域继续使用原快照。后台服务应按操作创建作用域。

HTTP 客户端可通过公开名称进行定制：

```csharp
using HH.YiDASDK;

builder.Services.AddHttpClient(YiDAHttpClientNames.Api, client =>
    client.Timeout = TimeSpan.FromSeconds(30));
builder.Services.AddHttpClient(YiDAHttpClientNames.Download, client =>
    client.Timeout = TimeSpan.FromMinutes(2));
```

## 请求、业务模型和分页

`yida:order` 表示应用别名 `yida` 下的表单别名 `order`；只写 `order` 时使用第一个应用。生成请求时会克隆配置模板，不会修改共享模板。

```csharp
using HH.YiDASDK;
using HH.YiDASDK.Core;
using System.ComponentModel;
using System.Text.Json.Serialization;

public sealed class OrderService(YiDAConfig config, IYiDAClient client)
{
    public async Task<List<GetFormDataByIdResultDomain<Order>>> QueryAsync(
        CancellationToken cancellationToken = default)
    {
        var request = config.Get<SearchFormDatasRequest<Order>>();
        request.SetPageSize(100);
        request.SetSearchFieldJsonWithType(new Order { Name = "学院 & 公司" });
        request.SetCreateFrom("2025-01-01 00:00:00");
        request.SetCreateTo("2025-12-31 23:59:59");
        request.OrderBy(order => order.Name!); // 使用 JsonPropertyName 中的宜搭字段名
        return await client.FetchAllAsync(request, cancellationToken);
    }

    public async Task<string?> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var request = config.Get<SaveFormDataRequest<Order>>();
        request.SetFormDataJson(order);
        var response = await client.ExecuteAsync(request, cancellationToken);
        return response.result;
    }
}

[Description("yida:order")]
public sealed class Order
{
    [JsonPropertyName("textField_name")]
    public string? Name { get; set; }
}
```

更新、删除、详情查询沿用原来的请求模型：

```csharp
var update = config.Get<UpdateFormDataRequest<Order>>();
update.SetFormInstId("instance-id");
update.SetUpdateFormDataJson(new Order { Name = "更新后的名称" });
await client.ExecuteAsync(update, cancellationToken);

var detail = config.Get<GetFormDataByIdRequest<Order>>("yida:order");
detail.SetFormInstId("instance-id");
var response = await client.ExecuteAsync(detail, cancellationToken);
var order = response.result?.formData;

var delete = config.Get<DeleteFormDataByIdRequest<Order>>();
delete.SetFormInstId("instance-id");
await client.ExecuteAsync(delete, cancellationToken);
```

日期范围等宜搭要求的 JSON 字符串仍可用 `JsonSerializer.Serialize(...)` 构造。`SetFormDataJson`、`SetFormData`、条件查询和排序支持 `JsonPropertyName`；序列化不会替换业务数据中的 `&`。表单编码由 HTTP 层统一处理，请勿预先 URL 编码。

`FetchAllAsync` 支持取消，保留最终不足一页的数据；查询范围仍需遵守宜搭接口本身的数据量限制。请求对象包含可变的分页状态，应为每次独立操作创建新请求。

获取表单控件模板：

```csharp
var request = config.Get<GetFormComponentDefinationListRequest>("yida:order");
var response = await client.ExecuteAsync(request, cancellationToken);
Console.WriteLine(response.OutputTemplateOfObject());
```

## 附件下载

先通过 `GetOpenUrlRequest` 将原始附件地址转换为临时免登地址，再调用响应的下载方法：

```csharp
var request = config.Get<GetOpenUrlRequest>("yida:order");
request.SetFileUrl("/ossFileHandle?fileId=原始附件地址参数");
request.SetTimeout(60000); // 临时地址有效期（毫秒），不是 HTTP 超时时间
var response = await client.ExecuteAsync(request, cancellationToken);

var path = await response.DownloadFileAsync("attachments", "订单.pdf", cancellationToken);

// 或直接读取、转存附件，调用方负责释放流。
await using var stream = await response.DownloadStreamAsync(cancellationToken);
await stream.CopyToAsync(destinationStream, cancellationToken);
```

`SetFileUrl` 支持完整 HTTP(S) 地址，也支持以 `/` 开头的宜搭相对地址。请保留原始 `&`、`+`、`$` 等字符。该接口不要求 `formUuid`，可以直接构造请求并设置 `appType`、`systemToken`、`userId`。

默认表单数据类型仍是 `IDictionary<string, object>`，其中非空 JSON 值现在为 `JsonElement`。附件字段可能是 JSON 字符串，也可能直接是数组，可按下面方式读取：

```csharp
using System.Text.Json;

var detail = config.Get<GetFormDataByIdRequest>("yida:order");
detail.SetFormInstId("instance-id");
var detailResponse = await client.ExecuteAsync(detail, cancellationToken);
if (detailResponse.result?.formData is { } data
    && data.TryGetValue("attachmentField_xxx", out var value)
    && value is JsonElement element
    && element.ValueKind is JsonValueKind.String or JsonValueKind.Array)
{
    using var attachments = JsonDocument.Parse(
        element.ValueKind == JsonValueKind.String ? element.GetString()! : element.GetRawText());
    foreach (var attachment in attachments.RootElement.EnumerateArray())
    {
        var request = config.Get<GetOpenUrlRequest>("yida:order");
        request.SetFileUrl(attachment.GetProperty("downloadUrl").GetString()!);
        var response = await client.ExecuteAsync(request, cancellationToken);
        await response.DownloadFileAsync(
            "attachments", attachment.GetProperty("name").GetString()!, cancellationToken);
    }
}
```

下载使用独立命名客户端，直接 GET 临时地址，不附加网关签名头。释放下载流时会同时释放 HTTP 响应和工厂创建的客户端。`DownloadFileAsync` 创建目标目录并覆盖同名文件；传入文件名应不含目录。请求失败或取消会抛出异常，复制中断时可能留下部分文件。

`ResponseHeadersRead` 让下载保持流式读取；`HttpClient.Timeout` 覆盖获取响应头，后续流读取和文件复制通过传入的取消令牌控制。

手动构造或重新反序列化的响应没有关联下载客户端，使用显式重载：

```csharp
using var http = httpClientFactory.CreateClient(YiDAHttpClientNames.Download);
await using var stream = await response.DownloadStreamAsync(http, cancellationToken);
// 也可调用 response.DownloadFileAsync(http, folder, fileName, cancellationToken)。
```

## 限流、重试和异常

SDK 在同一个 DI 容器内共享独立的限流器，使用 `SlidingWindowRateLimiter`：1 秒窗口、3 个许可、10 个时间片，等待队列按先后顺序处理。限流等待、重试等待、HTTP 请求、分页和下载均支持取消，不再阻塞线程。

`ClientConfig.RetryLimit` 是最大总尝试次数，包含第一次请求，默认 3，最小 1；`RetryDelayMilliseconds` 是两次尝试之间的固定等待时间，默认 500 毫秒。每次重试都重新构造并签名请求，最后一次失败后直接抛出异常。

自动重试仅针对宜搭业务限流码 `TIANSHU_000061`，以及错误码 `100` 且 `Message` 包含 `timeout` 的情况。签名错误和其他永久业务错误立即抛出 `YiDAException`。异常的 `Response` 保留错误码及原始 `body`，`StatusCode` 保留 HTTP 状态码。非成功 HTTP 响应中合法的宜搭错误 JSON 同样会被解析。

网络异常、HTTP 层超时和无有效宜搭错误 JSON 的 HTTP 失败会直接传播，不自动重试写入类 POST。调用方主动取消会得到 `OperationCanceledException`。SDK 日志只记录业务重试信息，不记录签名、配置密钥、响应正文或临时附件地址。

## 从旧版本迁移

| 旧用法 | 新用法 |
| --- | --- |
| `netstandard2.1`、预览语言版本 | `net10.0`、稳定 C# 14 |
| `Flurl.Http`、间接 Newtonsoft 依赖 | 原生 HTTP、System.Text.Json |
| SDK 注册时自行加载配置文件 | 宿主加载，调用 `AddYiDASdk(configuration.GetSection("YiDAConfig"))` |
| `AddYiDASDKSetup` | 保留为过时兼容入口，但不再加载文件 |
| `[JsonProperty("field")]` | `[JsonPropertyName("field")]` |
| `JsonConvert.SerializeObject` | `JsonSerializer.Serialize` |
| `JArray` / `JObject` | `JsonElement` 或 `JsonNode` / `JsonArray` |
| 单例 `YiDAConfig` | Scoped 配置快照，新作用域取得重载值 |
| `ExecuteAsync(request)`、`FetchAllAsync(request)` | 原调用仍可用，也可追加取消令牌 |

自定义 `IYiDAClient` 实现需要更新方法签名，增加 `CancellationToken cancellationToken = default`。直接构造 `DefaultYiDAClient` 的代码需要适配新的微软组件依赖，推荐通过 DI 获取 `IYiDAClient`。可能缺失的响应字段已补充可空标注，调用方应检查 `result` / `formData`。

## 支持的接口

| 类型 | 请求 |
| --- | --- |
| 新增表单实例 | `SaveFormDataRequest` |
| 更新表单数据 | `UpdateFormDataRequest` |
| 删除表单实例 | `DeleteFormDataByIdRequest` |
| 查询实例详情 | `GetFormDataByIdRequest` |
| 搜索实例 ID | `SearchFormDataIdsRequest` |
| 搜索实例详情、分页 | `SearchFormDatasRequest` |
| 获取表单定义 | `GetFormComponentDefinationListRequest` |
| 发起流程实例 | `StartInstanceRequest` |
| 获取附件临时地址 | `GetOpenUrlRequest` |
| 获取页面列表 | `ListNavigationByFormTypeRequest` |

扩展新接口时仍继承 `BaseYiDARequest<TResponse>` 和 `YiDAResponse`，实现 `GetUrl()`，按需覆盖 `Validate()`。

## 验证

```powershell
dotnet build HH.Integration.slnx
dotnet test HH.Integration.slnx
```

`tests/HH.YiDASDK.Tests` 使用模拟 HTTP 处理器验证签名与编码、JSON 映射、并发、配置校验与重载、错误与重试、取消、分页和附件生命周期，不需要真实宜搭凭据。
