using System.Globalization;
using System.Text.Json;

namespace HH.YiDASDK;

internal static class DictionaryUtils
{
    /// <summary>签名和表单使用同一份参数，排序与数值格式不受当前语言环境影响。</summary>
    public static IDictionary<string, string> ToDictionaryOfStringValue(this object obj)
        => obj.GetType().GetProperties()
            .Where(property => property.GetIndexParameters().Length == 0)
            .OrderBy(property => property.Name, StringComparer.Ordinal)
            .ToDictionary(property => property.Name, property =>
            {
                var value = property.GetValue(obj);
                return value is IFormattable formattable
                    ? formattable.ToString(null, CultureInfo.InvariantCulture)
                    : value?.ToString() ?? string.Empty;
            });

    /// <summary>使用相同的 JSON 配置，支持业务模型中的 JsonPropertyName。</summary>
    public static IDictionary<string, object> ToDictionaryOfObjectValue(this object obj)
        => JsonSerializer.SerializeToElement(obj, JsonUtils.Options)
            .EnumerateObject()
            .ToDictionary(property => property.Name, property => (object)property.Value.Clone());

    public static string GetKeyValuePairsString(this IDictionary<string, string> pairs, string character = "=")
        => string.Join('&', pairs.Select(pair => $"{pair.Key}{character}{pair.Value}"));
}
