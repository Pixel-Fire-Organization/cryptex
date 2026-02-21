using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;

namespace JavaScriptTranspiler.Data.Statements;

public class ArrowFunctionExpression : IExpression
{
    public string Type => "ArrowFunctionExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("params")]
    public List<IPattern> Params { get; set; }

    [JsonPropertyName("body")]
    public INode Body { get; set; } // BlockStatement or IExpression

    [JsonPropertyName("expression")]
    public bool Expression { get; set; } // True if `x => x * 2` (no block)
}