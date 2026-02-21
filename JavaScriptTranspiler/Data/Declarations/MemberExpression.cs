using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;

namespace JavaScriptTranspiler.Data.Declarations;

public class MemberExpression : IExpression, IPattern
{
    public string Type => "MemberExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("object")]
    public IExpression Object { get; set; }

    [JsonPropertyName("property")]
    public IExpression Property { get; set; }

    [JsonPropertyName("computed")]
    public bool Computed { get; set; } // true for obj[prop], false for obj.prop
}