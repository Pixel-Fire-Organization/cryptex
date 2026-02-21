using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class BinaryExpression : IExpression
{
    public string Type => "BinaryExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("left")]
    public IExpression Left { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("right")]
    public IExpression Right { get; set; }
}