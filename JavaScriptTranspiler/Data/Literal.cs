using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace JavaScriptTranspiler.Data;

public class Literal : IExpression
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }

    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }

    [JsonPropertyName("value"), JsonConverter(typeof(LiteralValueConverter))]
    public object? Value { get; set; }
}

public class LiteralValueConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();

            // Check if the string is formatted as a regex (example: /pattern/flags); no -> return the value.
            if (stringValue is null || !stringValue.StartsWith('/') || stringValue.LastIndexOf('/') <= 0) 
                return stringValue;
            
            var lastSlashIndex = stringValue.LastIndexOf('/');
            var pattern = stringValue.Substring(1, lastSlashIndex - 1);
            var optionsFlag = stringValue.Substring(lastSlashIndex + 1);

            // Convert regex flags if necessary (e.g., case-insensitive)
            var regexOptions = RegexOptions.None;
            if (optionsFlag.Contains('i'))
                regexOptions |= RegexOptions.IgnoreCase;

            return new Regex(pattern, regexOptions);

        }

        if (reader.TokenType == JsonTokenType.Number)
            return reader.GetDouble();
        if (reader.TokenType is JsonTokenType.True or JsonTokenType.False)
            return reader.GetBoolean();
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        throw new JsonException("Unsupported value type");
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case string s:
                writer.WriteStringValue(s);

                break;
            case double d:
                writer.WriteNumberValue(d);

                break;
            case bool b:
                writer.WriteBooleanValue(b);

                break;
            case Regex regex:
                // Serialize regex back to string format (e.g., /pattern/flags)
                var optionsFlag = regex.Options.HasFlag(RegexOptions.IgnoreCase) ? "i" : "";
                writer.WriteStringValue($"/{regex}/{optionsFlag}");

                break;
            case null:
                writer.WriteNullValue();

                break;
            default:
                throw new NotSupportedException("Unsupported value type");
        }
    }
}