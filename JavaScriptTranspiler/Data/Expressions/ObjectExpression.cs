using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Expressions;

public class ObjectExpression : IExpression
{
    public string Type => "ObjectExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("properties")]
    public List<INode> Properties { get; set; } // Can be Property or SpreadElement
}