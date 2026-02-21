using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class UpdateExpression : IExpression
{
    public string Type => "UpdateExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("argument")]
    public IExpression Argument { get; set; }

    [JsonPropertyName("prefix")]
    public bool Prefix { get; set; }
}