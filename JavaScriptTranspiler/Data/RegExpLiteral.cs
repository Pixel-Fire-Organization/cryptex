using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class RegExpLiteral : IExpression
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }
    [JsonPropertyName("regex")] public RegExp Regex { get; }
}

public record struct RegExp(string pattern, string flags);