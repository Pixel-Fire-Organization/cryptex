using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Expressions;

public class ArrayExpression : IExpression
{
    public string Type => "ArrayExpression";
    public int Start { get; set; }
    public int End { get; set; }

    // Can contain nulls for sparse arrays: [1, , 3]
    [JsonPropertyName("elements")]
    public List<IExpression> Elements { get; set; } 
}