# ZM.YiDA 宜搭SDK-NetCore


> [**宜搭官方接口文档**](https://www.yuque.com/yida/support/agb8im "宜搭官方接口文档")


## 🎉 使用

#### 基础使用
```cs
var client = new DefaultYiDAClient();
var req = new XXXRequest();
var rsp = client.Execute(req);
Console.WriteLine(rsp.body);

```
 


#### 扩展使用
###### 根据宜搭请求参数的特点,您可以使用 DefaultYiDARequest 以及扩展方法实现以下功能
- **SetFormUuid()** 构建不同表单的请求对象
- **ConvertTo()** 减少必要参数赋值

```cs
var client = new DefaultYiDAClient();

// 定义基础的请求对象
var defaultReq = new DefaultYiDARequest("应用编码","应用密钥","钉钉用户ID");

// 定义具体表单请求对象
var aFromReq = defaultReq.SetFormUuid("FORM-A表单ID");
var bFromReq = defaultReq.SetFormUuid("FORM-B表单ID");
 
// 转换具体请求对象
var req = aFromReq.ConvertTo<XXXRequest>();
var rsp = client.Execute(req);
Console.WriteLine(rsp.body);

```

## 🎈 支持

```cs
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

```


## 🧩 依赖

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netcoreapp2.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="12.0.3" />
    <PackageReference Include="RestSharp" Version="106.12.0" />
  </ItemGroup>
</Project>

```

## 🕹 扩展


```cs
// XXXResponse 需要继承YiDAResponse
// result (对象) 和 content (集合) 为预置参数 
// 会参与进行反序列化
public partial class XXXResponse : YiDAResponse
{  
    public T result { get; set; }
    public IEnumerable<T> content { get; set; }
}

```

```cs
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



## 📌 交流


- 【 DingTalk 】**zimotalk**
- 【 WeChat 】**zimoa927**

