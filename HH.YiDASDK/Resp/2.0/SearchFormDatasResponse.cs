namespace HH.YiDASDK;

/// <summary>
/// 2.6 根据条件搜索表单实例详情列表
/// </summary>
public partial class SearchFormDatasResponse<D> : YiDAResponse
{
    /// <summary>
    /// 实例详情列表 对象
    /// </summary>
    public SearchFormDatasResultDomain<D> result { get; set; }
}
/// <summary>
/// 实例详情列表 对象
/// </summary>
public class SearchFormDatasResultDomain<D> : TopPageObject
{
    /// <summary>
    /// 实例详情 集合
    /// </summary>
    public List<GetFormDataByIdResultDomain<D>> data { get; set; }
}
