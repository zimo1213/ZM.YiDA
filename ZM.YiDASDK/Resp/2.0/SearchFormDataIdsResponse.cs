using System.Collections.Generic;
using System.Linq;

namespace ZM.YiDASDK
{
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
        public IEnumerable<string> data { get; set; }
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
        IEnumerable<string> GetInstanceIds();
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
        public string GetInstanceIdFirst() => GetInstanceIds()?.FirstOrDefault();
        /// <summary>
        /// 获取 实例ID列表
        /// </summary>
        /// <returns>可为空的string集合</returns>
        public IEnumerable<string> GetInstanceIds()
        {
            if (!success || result == null || result.data == null || result.data.Count() == 0) { return null; }

            return result.data;
        }
    }
}
