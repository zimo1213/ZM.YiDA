namespace HH.YiDASDK;

/// <summary>
/// TOP基础分页对象。
/// </summary>
public abstract class TopPageObject : TopObject
{
    /// <summary>
    /// 分页游标
    /// </summary>
    public int idCursor { get; set; }
    /// <summary>
    /// 符合条件的实例总数
    /// </summary>
    public int totalCount { get; set; }
    /// <summary>
    /// 当前页
    /// </summary>
    public int currentPage { get; set; }
}
