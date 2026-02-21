using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Expressions;

public class SpreadElement : IExpression // Strictly, ESTree maps this as INode, but Acorn often treats it as an expression in arrays
{
    public string Type => "SpreadElement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("argument")]
    public IExpression Argument { get; set; }
}