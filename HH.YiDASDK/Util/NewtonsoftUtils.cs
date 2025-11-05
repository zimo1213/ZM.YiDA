using Newtonsoft.Json;

namespace HH.YiDASDK;

/// <summary>
/// 序列化扩展
/// </summary>
internal static class NewtonsoftUtils
{
    /// <summary>
    /// 序列配置 空转空字符
    /// </summary>
    static readonly JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    /// <summary>
    /// 正序列
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize(this object obj) => JsonConvert.SerializeObject(obj, Formatting.None, settings)?.Replace("&", " ");
    /// <summary>
    /// 反序列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T Deserialize<T>(this string obj) => JsonConvert.DeserializeObject<T>(obj);
}
