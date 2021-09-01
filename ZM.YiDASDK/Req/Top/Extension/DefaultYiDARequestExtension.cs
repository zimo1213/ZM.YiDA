using System;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 默认类型扩展
    /// </summary>
    public static class DefaultYiDARequestExtension
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <typeparam name="D">转换目标类型</typeparam>
        /// <param name="req">基础载体</param>
        /// <returns>目标对象</returns>
        public static D ConvertTo<D>(this DefaultYiDARequest req) => req.Serialize().Deserialize<D>();

        /// <summary>
        /// 设置时间 - 转 Timestamp
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>Timestamp</returns>
        public static string SetDateTime(this DateTime dt)
        {
            return dt.UnixToTimestamp().ToString();
        }
    }
}
