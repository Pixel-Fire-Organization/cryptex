using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class IfStatement : IStatement
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }

    [JsonPropertyName("test")] public IExpression Test { get; }
    [JsonPropertyName("consequent")] public IStatement Consequent { get; }
    [JsonPropertyName("alternative")] public IStatement? Alternative { get; }
}