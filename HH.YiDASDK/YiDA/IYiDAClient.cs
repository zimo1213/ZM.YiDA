namespace HH.YiDASDK;

/// <summary>
/// SDK对外接口
/// </summary>
public interface IYiDAClient
{
    /// <summary>
    /// 执行请求
    /// </summary>
    /// <typeparam name="T">基础返回抽象对象</typeparam>
    /// <param name="request">请求对象</param>
    /// <returns></returns>
    Task<T> ExecuteAsync<T>(IYiDARequest<T> request) where T : YiDAResponse;
}