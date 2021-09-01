namespace ZM.YiDASDK
{
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
        /// <param name="token">身份对象,为空调用内置配置</param>
        /// <returns></returns>
        T Execute<T>(IYiDARequest<T> request, TokenRequest token = null) where T : YiDAResponse;
    }
}
