namespace ZM.YiDASDK
{
    /// <summary>
    /// 调试 输出扩展
    /// </summary>
    internal static class OutputUtils
    {
        /// <summary>
        /// 输出对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Output<T>(this T obj)
        {
            System.Diagnostics.Debug.WriteLine($"zm.yidasdk  {obj}");
            return obj;
        }
        /// <summary>
        /// 输出对象 带描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T Output<T>(this T obj, string message)
        {
            System.Diagnostics.Debug.WriteLine($"zm.yidasdk  {message}：{obj}");
            return obj;
        }
    }
}
