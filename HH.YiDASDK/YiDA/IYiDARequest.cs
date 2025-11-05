namespace HH.YiDASDK;

/// <summary>
/// 基础请求接口
/// </summary>
/// <typeparam name="T">基础返回抽象对象</typeparam>
public interface IYiDARequest<out T> where T : YiDAResponse
{
    /// <summary>
    /// 参数验证
    /// </summary>
    void Validate();
    /// <summary>
    /// 接口地址
    /// </summary>
    /// <returns></returns>
    string GetUrl();
}
