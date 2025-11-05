namespace HH.YiDASDK;

/// <summary>
/// 2.7 获取表单定义
/// </summary>
public partial class GetFormComponentDefinationListResponse : YiDAResponse
{
    /// <summary>
    /// 表单定义 对象集合
    /// </summary>
    public List<FormComponentDefinationListContentDomain> content { get; set; }
}
/// <summary>
/// 表单定义 对象集合
/// </summary>
public class FormComponentDefinationListContentDomain : TopObject
{
    /// <summary>
    /// 标题
    /// </summary>
    public string label { get; set; }
    /// <summary>
    /// 唯一标识
    /// </summary>
    public string key { get; set; }
    /// <summary>
    /// 父标识
    /// </summary>
    public string parentId { get; set; }
    /// <summary>
    /// 组件名称
    /// </summary>
    public string componentName { get; set; }
    /// <summary>
    /// 组件描述
    /// </summary>
    public string desc => label?.Deserialize<FormComponentDefinationListContentLabelDomain>()?.zh_CN;
}
/// <summary>
/// 组件描述 对象
/// </summary>
public class FormComponentDefinationListContentLabelDomain : TopLanguageObject
{
}
