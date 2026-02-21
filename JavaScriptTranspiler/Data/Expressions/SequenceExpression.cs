using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class SequenceExpression : IExpression
{
    public string Type => "SequenceExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("expressions")]
    public List<IExpression> Expressions { get; set; }
}