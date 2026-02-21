using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data;

public class Property : INode
{
    public string Type => "Property";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("key")]
    public IExpression Key { get; set; }

    [JsonPropertyName("value")]
    public INode Value { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("method")]
    public bool Method { get; set; }

    [JsonPropertyName("shorthand")]
    public bool Shorthand { get; set; }

    [JsonPropertyName("computed")]
    public bool Computed { get; set; }
}