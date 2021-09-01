# ZM.YiDA
宜搭SDK-NetCore-aliwork.com


## 🎉 使用


```cs

// 定义基础请求参数
 var defaultReq = new DefaultYiDARequest(
 "应用编码",
 "应用密钥",
 "钉钉用户ID");
 
 // 定义一张表单
 var fromReq = defaultReq.SetFormUuid("FORM-表单ID");
 
 // 请求具体方法
 var req = fromReq.ConvertTo<XXXRequest>();
 var rsp = client.Execute(req);
 Console.WriteLine(rsp.body);

```



## 🕹 扩展

```cs

// XXXResponse 需要继承YiDAResponse
// result (对象) 和 content (集合) 为预置参数 对象会进行反序列化
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
	public override string GetUrl() 
		=> "/yida_vpc/process/startInstance.json";
 
	public override void Validate()
		=> base.Validate();
		
	public string 其他请求参数 { get; set; }
}


```