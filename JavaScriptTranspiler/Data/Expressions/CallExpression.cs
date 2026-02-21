using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class CallExpression : IExpression
{
    public string Type => "CallExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("callee")]
    public IExpression Callee { get; set; }

    [JsonPropertyName("arguments")]
    public List<IExpression> Arguments { get; set; } // Can include SpreadElement

    [JsonPropertyName("optional")]
    public bool Optional { get; set; } // True for optional chaining: `myFunc?.()`
}