using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class WithStatement : IStatement
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }

    [JsonPropertyName("object")] public IExpression Object { get; }
    [JsonPropertyName("body")] public IStatement Body { get; }
}