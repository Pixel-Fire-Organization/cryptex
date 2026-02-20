using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class ThrowStatement : IStatement
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }

    [JsonPropertyName("argument")] public IExpression Argument { get; }
}