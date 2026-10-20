> 当前sdk是旧版本,钉钉开放平台已经支持[宜搭(新)](https://open.dingtalk.com/document/orgapp/overview-yida),该项目仅仅用于测试工作流以及代码检测


# 宜搭SDK-NetCore-netstandard2.1


> [**宜搭官方接口文档(旧)**](https://www.yuque.com/yida/support/agb8im "宜搭官方接口文档")


- 支持多个配置
- 加入了Limiter限制qps,1秒钟请求3次
- 依赖走nuget

## 🎉 使用

在`Program.cs`追加一行

```csharp
builder.Services.AddYiDASDKSetup(builder.Configuration);
```

将`appsettings.YiDA.json` 复制到你的项目,和`appsettings.json`平级就可以使用了



## 🎡配置

- 如何设置?

进入[宜搭](https://aliwork.com/) ,切换到`我的应用`,进入你需要操作的应用

切换到`应用设置`>`部署运维`>复制**应用编码**/**应用密钥**/**当前登录人ID**

他们分别对应YiDAConfig.RequestMap.yida.request

下面的表单**页面编码**对应YiDAConfig.RequestMap.yida.formUuidMap



- 如何构建domain

```csharp
var req = yiDAConfig.Get<GetFormComponentDefinationListRequest>("yida:order");
var rsp = await yiDAClient.ExecuteAsync(req);
await Console.Out.WriteLineAsync(rsp.OutputTemplateOfObject());
```


- 就能获取到这个表单所有的控件名称和类型
  
```csharp
var template = new {
    textField_ugyfv0a="" , // 规格 | TextField
};
```

- 改造你的 domain
  
```csharp
[Description("yida:order")]
internal sealed class OrderYiDADomain
{
    /// <summary>
    /// 规格
    /// </summary>
    [JsonProperty("textField_ugyfv0a")] public string? Sku { get; set; }
}
```



## ✨ 示例

```csharp
using HH.YiDASDK;
using HH.YiDASDK.Core;
using HH.YiDASDK.Util;
using Newtonsoft.Json;
using System.ComponentModel;

namespace NetDemo.App;
internal class Demo(YiDAConfig yiDAConfig, IYiDAClient yiDAClient)
{
    // 模板
    private async Task YiDATemplate()
    {
        var req = yiDAConfig.Get<GetFormComponentDefinationListRequest>("yida:order");
        var rsp = await yiDAClient.ExecuteAsync(req);
        await Console.Out.WriteLineAsync(rsp.OutputTemplateOfObject());
    }
    // 查询
    public async Task YiDAQuery()
    {
        var req = yiDAConfig.Get<SearchFormDatasRequest<OrderYiDADomain>>();
        var rsp = await yiDAClient.ExecuteAsync(req);
        await Console.Out.WriteLineAsync(rsp.result.data.Count().ToString());
    }
    // 查询带条件
    private async Task YiDAQueryWithSearch()
    {
        var req = yiDAConfig.Get<SearchFormDatasRequest<OrderYiDADomain>>();
        req.SetPageSize(100);
        #region 表单内查询
        // 按条件查询,表单内的
        var start = Convert.ToDateTime("2025-01-09");
        var end = Convert.ToDateTime("2025-01-09").AddDays(1).AddSeconds(-1);
        req.SetSearchFieldJsonWithType(new Dictionary<string, string> {
            // 日期范围
            { "dateField_ugbn574", JsonConvert.SerializeObject(new long[]{ 
                start.ToUnixTimeMilliseconds(),end.ToUnixTimeMilliseconds()})},
            // 字符串搜索
            { "textField_gcaq4fa", "学院"},
            // 更多参考宜搭官方文档 https://www.yuque.com/yida/support/agb8im   
            // 条件搜索传参格式说明
        });
        #endregion

        #region 表单创建的时间
        req.SetCreateFrom($"2024-01-01 00:00:00");
        req.SetCreateTo($"2025-12-31 23:59:59");
        #endregion

        var rsp = await yiDAClient.ExecuteAsync(req);
        var data = rsp.result.data;
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <returns></returns>
    private async Task YiDAQueryWithSearchUseDomain()
    {
        var req = yiDAConfig.Get<SearchFormDatasRequest<OrderYiDADomain>>();
        req.SetPageSize(100);
        #region 表单内查询
        // 按条件查询,表单内的
        DateTime start = Convert.ToDateTime("2025-01-09");
        DateTime end = Convert.ToDateTime("2025-01-09").AddDays(1).AddSeconds(-1);

        OrderYiDADomain domain = new();
        domain.CorpName = "学院";
        domain.Date = JsonConvert.SerializeObject(new long[] {
            start.ToUnixTimeMilliseconds(), end.ToUnixTimeMilliseconds() });
        req.SetSearchFieldJsonWithType(domain);
        #endregion

        #region 表单创建的时间
        req.SetCreateFrom($"2024-01-01 00:00:00");
        req.SetCreateTo($"2025-12-31 23:59:59");
        #endregion

        var rsp = await yiDAClient.ExecuteAsync(req);
        var data = rsp.result.data;
    }
	// 查询所有
    private async Task YiDAQueryWithFetchAll()
    {
        var req = yiDAConfig.Get<SearchFormDatasRequest<OrderYiDADomain>>();
        req.SetPageSize(100);

        #region 表单创建的时间
        req.SetCreateFrom($"2025-01-01 00:00:00");
        req.SetCreateTo($"2025-01-31 23:59:59");
        #endregion
        // 注意:单次查询不能超过3万条数据
        var results = await yiDAClient.FetchAllAsync(req);
    }
    // 新增
     private async Task YiDACreate()
     {
         OrderYiDADomain domain = new() { Sku = "abc" };

         var req = yiDAConfig.Get<SaveFormDataRequest<OrderYiDADomain>>();
         req.SetFormDataJson(domain);
         var rsp = await yiDAClient.ExecuteAsync(req);
     }
    // 修改
    private async Task YiDAUpdate()
    {
        OrderYiDADomain domain = new() { Sku = "abc" };

        var req = yiDAConfig.Get<UpdateFormDataRequest<OrderYiDADomain>>();
        req.SetUpdateFormDataJson(domain);
        req.SetFormInstId("instanceId");
        var rsp = await yiDAClient.ExecuteAsync(req);
    }
    // 删除
    private async Task YiDADelete()
    {
        var req = yiDAConfig.Get<DeleteFormDataByIdRequest<OrderYiDADomain>>();
        req.SetFormInstId("instanceId");
        var rsp = await yiDAClient.ExecuteAsync(req);
    }
}

[Description("yida:order")]
internal sealed class OrderYiDADomain
{
    [JsonProperty("textField_ugyfv0a")] public string? Sku { get; set; }
    [JsonProperty("textField_gcaq4fa")] public string? CorpName { get; set; }
    [JsonProperty("dateField_ugbn574")] public string? Date { get; set; }
    
    // 下拉选择如何获取value?,在字段后面加[_id]
    [JsonProperty("textField_xxx_id")] public string? xxxId { get; set; }
}
```



## 📎 附件下载

通过 `GetOpenUrlRequest` 将附件地址转换为临时免登地址，再调用响应对象的 `DownloadFileAsync` 保存文件，或调用 `DownloadStreamAsync` 获取文件流。请求沿用 `ExecuteAsync` 的签名、限流和重试逻辑，文件下载使用临时地址直接 GET。

```csharp
using HH.YiDASDK;
using Newtonsoft.Json.Linq;

// order 为配置中的表单别名，这里使用第一个应用配置。
var detailReq = yiDAConfig.Get<GetFormDataByIdRequest>("order");
detailReq.SetFormInstId("你的实例ID");
var detailRsp = await yiDAClient.ExecuteAsync(detailReq);

// 默认查询类型的附件字段通常是 JSON 字符串，也支持反序列化后的 JArray。
if (detailRsp.result.formData.TryGetValue("attachmentField_xxx", out var value))
{
    var attachments = JArray.Parse(value?.ToString() ?? "[]");
    foreach (var attachment in attachments)
    {
        var req = yiDAConfig.Get<GetOpenUrlRequest>("order");
        req.SetFileUrl(attachment.Value<string>("downloadUrl")!);
        req.SetTimeout(60000); // 临时地址有效期，单位毫秒，默认一分钟

        var rsp = await yiDAClient.ExecuteAsync(req);
        var localPath = await rsp.DownloadFileAsync(
            "attachments", attachment.Value<string>("name")!);
        Console.WriteLine(localPath);
    }
}
```

`SetFileUrl` 支持完整 HTTP/HTTPS 地址，也支持 `/ossFileHandle?...` 形式的相对地址；相对地址会补全为 `https://www.aliwork.com/...`。请直接传入原始附件地址，保留 `&`、`$` 等字符，不要提前进行 URL 编码。该接口不要求 `formUuid`，也可以直接构造 `GetOpenUrlRequest`，设置 `appType`、`systemToken`、`userId` 后调用。

如果需要将附件转存到其他系统，可以使用文件流：

```csharp
var req = yiDAConfig.Get<GetOpenUrlRequest>("order");
req.SetFileUrl(attachmentDownloadUrl);
var rsp = await yiDAClient.ExecuteAsync(req);
using var stream = await rsp.DownloadStreamAsync(cancellationToken);
// 在此读取或转存 stream，由调用方负责释放。
```

两种下载方法均支持 `CancellationToken`。`DownloadFileAsync` 自动创建目标目录并覆盖同名文件；下载异常会抛出。临时地址过期后需要重新执行 `GetOpenUrlRequest` 获取地址。

## 🎈 支持

```csharp
3. 流程接口
3.1 发起新的流程实例 StartInstanceRequest

2. 表单接口
2.1 新增表单实例 SaveFormDataRequest
2.2 更新表单中指定组件值 UpdateFormDataRequest
2.3 删除表单实例 DeleteFormDataByIdRequest
2.4 根据表单 ID 查询实例详情 GetFormDataByIdRequest
2.5 根据条件搜索表单实例 ID 列表 SearchFormDataIdsRequest
2.6 根据条件搜索表单实例详情列表 SearchFormDatasRequest
2.7 获取表单定义 GetFormComponentDefinationListRequest

5. 其他接口
5.1 附件地址转临时免登地址 GetOpenUrlRequest
    下载附件流 GetOpenUrlResponse.DownloadStreamAsync
    保存附件到本地 GetOpenUrlResponse.DownloadFileAsync
5.3 获取应用下的页面列表 ListNavigationByFormTypeRequest

```



## 🕹 扩展


```csharp
// XXXResponse 需要继承YiDAResponse
// result (对象) 和 content (集合) 为预置参数 
// 会参与进行反序列化
public partial class XXXResponse : YiDAResponse
{  
    public T result { get; set; }
    public List<T> content { get; set; }
}

```

```csharp
// XXXRequest 需要继承 BaseYiDARequest 
// 必须实现 具体的宜搭请求方法GetUrl()
// 可实现 验证方法Validate()
// 可定义其他接口的请求参数
public class XXXRequest : BaseYiDARequest<XXXResponse>
{
    public override string GetUrl() => "/yida_vpc/process/startInstance.json";
 
    public override void Validate() => base.Validate();
		
    public T 其他请求参数 { get; set; }
}

```
