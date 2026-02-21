using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class AwaitExpression : IExpression
{
    public string Type => "AwaitExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("argument")]
    public IExpression Argument { get; set; }
}