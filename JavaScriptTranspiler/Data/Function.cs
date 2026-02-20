using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class Function : INode
{
    [JsonPropertyName("id")] public Identifier? Id { get; }
    [JsonPropertyName("params")] public IPattern Params { get; }
    [JsonPropertyName("body")] public FunctionBody Body;
    
    [JsonPropertyName("type")] public string Type { get; }
    public long Start { get; }
    public long End { get; }
    //[JsonPropertyName("loc")] public SourceLocation Loc { get; }
}