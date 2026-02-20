using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class DebuggerStatement : IStatement
{
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }
}