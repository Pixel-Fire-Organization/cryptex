using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class UnaryExpression : IExpression
{
    public string Type => "UnaryExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("prefix")]
    public bool Prefix { get; set; }

    [JsonPropertyName("argument")]
    public IExpression Argument { get; set; }
}