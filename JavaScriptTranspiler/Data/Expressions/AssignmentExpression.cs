using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class AssignmentExpression : IExpression
{
    public string Type => "AssignmentExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("left")]
    public INode Left { get; set; } // Can be IPattern or IExpression (like MemberExpression)

    [JsonPropertyName("right")]
    public IExpression Right { get; set; }
}