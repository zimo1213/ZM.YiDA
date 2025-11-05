namespace HH.YiDASDK;

/// <summary>
/// 3.1 发起新的流程实例
/// </summary>
public partial class ListNavigationByFormTypeResponse : YiDAResponse
{
    public List<ListNavigationByFormTypeResultDomain> result { get; set; }
}
/// <summary>
/// 表单实例 ID 对象
/// </summary>
public class ListNavigationByFormTypeResultDomain : TopObject
{
    public string formUuid { get; set; }
    public string processCode { get; set; }
    public ListNavigationByFormTypeResulttLabelDomain title { get; set; }
    public string desc => title?.zh_CN;
}
/// <summary>
/// 组件描述 对象
/// </summary>
public class ListNavigationByFormTypeResulttLabelDomain : TopLanguageObject
{
}