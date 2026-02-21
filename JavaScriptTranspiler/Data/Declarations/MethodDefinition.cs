using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class MethodDefinition : INode
{
    public string Type => "MethodDefinition";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("key")]
    public IExpression Key { get; set; }

    [JsonPropertyName("value")]
    public IExpression Value { get; set; } // FunctionExpression

    [JsonPropertyName("kind")]
    public string Kind { get; set; } // "constructor", "method", "get", or "set"

    [JsonPropertyName("computed")]
    public bool Computed { get; set; }

    [JsonPropertyName("static")]
    public bool Static { get; set; }
}