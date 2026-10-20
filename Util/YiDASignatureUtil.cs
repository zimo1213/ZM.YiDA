using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HH.YiDASDK;

internal static class YiDASignatureUtil
{
    public static string Signature(IDictionary<string, string> pairs, string timestamp, string nonce, string uri, string secret)
    {
        string canonical = $"POST\n{timestamp}\n{nonce}\n{uri}\n{pairs.GetKeyValuePairsString()}";
        return Convert.ToBase64String(HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(canonical)));
    }

    public static string FormatIso8601Date(this DateTime date)
        => date.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);

    public static string Iso8601Date() => DateTime.UtcNow.FormatIso8601Date();
}
