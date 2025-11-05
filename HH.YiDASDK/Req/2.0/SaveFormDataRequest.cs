namespace HH.YiDASDK;

/// <summary>
/// 2.1 新增表单实例
/// </summary>
public class SaveFormDataRequest<D> : SaveFormDataRequest where D : class;
public class SaveFormDataRequest : BaseYiDARequest<SaveFormDataResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/form/saveFormData.json";

    /// <summary>
    /// 表单数据
    /// </summary>
    public string formDataJson { get; private set; }

    /// <summary>
    /// 设置 表单数据
    /// </summary>
    /// <param name="str">字符串</param>
    public void SetFormDataJson(string str) { this.formDataJson = str; }
    /// <summary>
    /// 设置 表单数据 对象序列化
    /// </summary>
    /// <param name="obj">object</param>
    public void SetFormDataJson(object obj) { this.formDataJson = obj.Serialize(); }
    /// <summary>
    /// 设置 表单数据 对象转字典后序列化
    /// </summary>
    /// <param name="obj">object</param>
    public void SetFormData(object obj) { this.formDataJson = obj?.ToDictionaryOfObjectValue()?.Serialize(); }
    /// <summary>
    /// 设置 表单数据 字典序列化
    /// </summary>
    /// <param name="dictionary">字典对象</param>
    public void SetFormData(IDictionary<string, string> dictionary) { this.formDataJson = dictionary?.Serialize(); }
    /// <summary>
    /// 设置 表单数据 字典序列化
    /// </summary>
    /// <param name="dictionary">字典对象</param>
    public void SetFormData(IDictionary<string, object> dictionary) { this.formDataJson = dictionary?.Serialize(); }

    /// <summary>
    /// 数据验证
    /// </summary>
    public override void Validate()
    {
        formDataJson.ValidateRequired("formDataJson");

        base.Validate();
    }
}
