using System.Collections.Generic;
using System.Linq;

namespace ZM.YiDASDK
{
    /// <summary>
    ///  字典 扩展
    /// </summary>
    internal static class DictionaryUtils
    {
        /// <summary>
        /// 对象全部属性转字典
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionaryOfStringValue(this object obj)
            => obj.GetType().GetProperties().OrderBy(o => o.Name).ToDictionary(
                    q => q.Name,
                    q => q.GetValue(obj)?.ToString() ?? string.Empty);

        /// <summary>
        /// 对象全部属性转字典
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionaryOfObjectValue(this object obj)
            => obj.GetType().GetProperties().OrderBy(o => o.Name).ToDictionary(
                    q => q.Name,
                    q => q.GetValue(obj));

        /// <summary>
        /// 字典转字符串
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string GetKeyValuePairsString(this IDictionary<string, string> keyValuePairs, string character = "=")
        {
            string[] ret = keyValuePairs.Select(s => $"{s.Key}{character}{s.Value ?? string.Empty}").ToArray();
            return string.Join('&', ret);
        }
    }
}
