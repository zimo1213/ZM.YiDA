namespace HH.YiDASDK;

/// <summary>
/// 2.5 根据条件搜索表单实例 ID 列表
/// </summary>
public class SearchFormDataIdsRequest : BasePageYiDARequest<SearchFormDataIdsResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/form/searchFormDataIds.json";
}
