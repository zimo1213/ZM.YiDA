namespace HH.YiDASDK;

/// <summary>
/// 2.5 根据条件搜索表单实例 ID 列表
/// </summary>
public partial class SearchFormDataIdsResponse : YiDAResponse
{
    /// <summary>
    /// 表单实例 ID 对象
    /// </summary>
    public SearchFormDataIdsResultDomain result { get; set; }
}
/// <summary>
/// 表单实例 ID 对象
/// </summary>
public class SearchFormDataIdsResultDomain : TopPageObject
{
    /// <summary>
    /// 表单实例 ID 集合
    /// </summary>
    public List<string> data { get; set; }
}

/// <summary>
/// 2.5 根据条件搜索表单实例 ID 列表 扩展方法 接口
/// </summary>
public interface ISearchFormDataIdsResponse : IYiDAResponse<SearchFormDataIdsResponse>
{
    /// <summary>
    /// 实例ID列表
    /// </summary>
    /// <returns></returns>
    List<string> GetInstanceIds();
    /// <summary>
    /// 获取 第一条实例 ID
    /// </summary>
    /// <returns></returns>
    string GetInstanceIdFirst();
}
/// <summary>
/// 2.5 根据条件搜索表单实例 ID 列表 扩展方法 实现
/// </summary>
public partial class SearchFormDataIdsResponse : ISearchFormDataIdsResponse
{
    /// <summary>
    /// 获取 第一条实例 ID
    /// </summary>
    /// <returns></returns>
    public string? GetInstanceIdFirst() => GetInstanceIds()?.FirstOrDefault();
    /// <summary>
    /// 获取 实例ID列表
    /// </summary>
    /// <returns>可为空的string集合</returns>
    public List<string> GetInstanceIds()
    {
        if (!success || result is null || result.data is null || !result.data.Any()) { return null; }

        return result.data;
    }
}
