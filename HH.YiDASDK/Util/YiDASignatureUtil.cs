using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HH.YiDASDK;

/// <summary>
/// 宜搭 加密扩展
/// </summary>
internal static class YiDASignatureUtil
{
    /// <summary>
    /// ISO表达式
    /// </summary>
    private const string ISO8601_DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss'Z'";
    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="keyValuePairs"></param>
    /// <param name="timestamp"></param>
    /// <param name="nonce"></param>
    /// <param name="uri"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static string Signature(IDictionary<string, string> keyValuePairs, string timestamp, string nonce, string uri, string secret)
    {
        string canonical = $"POST\n{timestamp}\n{nonce}\n{uri}\n{keyValuePairs.GetKeyValuePairsString()}";
        return ComputeSignature(secret, canonical);
    }
    /// <summary>
    /// 执行签名
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="canonicalString"></param>
    /// <returns></returns>
    private static string ComputeSignature(string secret, string canonicalString)
    {
        byte[] signData = Sign(Encoding.UTF8.GetBytes(canonicalString), Encoding.UTF8.GetBytes(secret));
        return Convert.ToBase64String(signData);
    }
    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static byte[] Sign(byte[] key, byte[] data)
    {
        using (HMACSHA256 sha256 = new HMACSHA256(data))
        {
            return sha256.ComputeHash(key);
        }
    }
    /// <summary>
    /// 格式化ISO日期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string FormatIso8601Date(this DateTime date) => date.ToUniversalTime().ToString(ISO8601_DATE_FORMAT, CultureInfo.CreateSpecificCulture("en-US"));
    /// <summary>
    /// 获取ISO日期
    /// </summary>
    /// <returns></returns>
    public static string Iso8601Date() => DateTime.Now.FormatIso8601Date();
}
