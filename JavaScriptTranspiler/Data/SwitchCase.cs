using System.Text.Json.Serialization;

using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data;

public class SwitchCase : INode
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }

    [JsonPropertyName("test")] public IExpression? Test { get; }
    [JsonPropertyName("consequent")] public IStatement[] Consequent { get; }
}