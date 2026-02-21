using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class ConditionalExpression : IExpression
{
    public string Type => "ConditionalExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("test")]
    public IExpression Test { get; set; }

    [JsonPropertyName("consequent")]
    public IExpression Consequent { get; set; }

    [JsonPropertyName("alternate")]
    public IExpression Alternate { get; set; }
}