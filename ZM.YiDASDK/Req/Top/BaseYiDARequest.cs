namespace ZM.YiDASDK
{
    /// <summary>
    /// 基础请求抽象对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseYiDARequest<T> : DefaultYiDARequest, IYiDARequest<T> where T : YiDAResponse
    {
        /// <summary>
        /// 宜搭 具体请求方法
        /// </summary>
        /// <returns></returns>
        public abstract string GetUrl();
    }
}
