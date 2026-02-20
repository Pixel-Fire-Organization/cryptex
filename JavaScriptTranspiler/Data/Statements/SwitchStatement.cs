using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class SwitchStatement : IStatement
{
    [JsonPropertyName("type")]public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")]public SourceLocation Loc { get; }
    
    [JsonPropertyName("discriminant")] public IExpression Discriminant { get; }
    [JsonPropertyName("cases")] public SwitchCase[] Cases { get; }
}