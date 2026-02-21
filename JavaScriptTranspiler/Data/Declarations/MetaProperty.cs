using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class MetaProperty : IExpression
{
    public string Type => "MetaProperty";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("meta")]
    public Identifier Meta { get; set; }

    [JsonPropertyName("property")]
    public Identifier Property { get; set; }
}