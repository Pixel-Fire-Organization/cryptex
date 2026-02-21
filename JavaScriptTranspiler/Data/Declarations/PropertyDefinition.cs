using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class PropertyDefinition : INode // For class fields like `#privateField = 42;`
{
    public string Type => "PropertyDefinition";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("key")]
    public IExpression Key { get; set; } // Identifier or PrivateIdentifier

    [JsonPropertyName("value")]
    public IExpression Value { get; set; } // Nullable

    [JsonPropertyName("computed")]
    public bool Computed { get; set; }

    [JsonPropertyName("static")]
    public bool Static { get; set; }
}