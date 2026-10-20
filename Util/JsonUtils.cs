using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HH.YiDASDK;

internal static class JsonUtils
{
    internal static JsonSerializerOptions Options { get; } = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            // JSON 嵌入表单参数，HTTP 层负责 URL 编码；保留原始业务字符。
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        options.Converters.Add(new StringValueConverter());
        options.MakeReadOnly(populateMissingResolver: true);
        return options;
    }

    public static string Serialize(this object? value) => JsonSerializer.Serialize(value, Options);

    public static T Deserialize<T>(this string json)
        => JsonSerializer.Deserialize<T>(json, Options)
            ?? throw new JsonException($"Expected a non-null {typeof(T).Name}.");

    // 兼容宜搭将错误码、控件值等字符串字段返回为数字或布尔值的情况。
    private sealed class StringValueConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return reader.GetString();
            if (reader.TokenType is JsonTokenType.True or JsonTokenType.False)
                return reader.GetBoolean().ToString();
            if (reader.TokenType == JsonTokenType.Number)
            {
                using var document = JsonDocument.ParseValue(ref reader);
                return document.RootElement.GetRawText();
            }
            throw new JsonException("Expected a string, number or boolean.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            => writer.WriteStringValue(value);
    }
}
