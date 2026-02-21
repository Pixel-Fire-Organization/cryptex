using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class YieldExpression : IExpression
{
    public string Type => "YieldExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("argument")]
    public IExpression Argument { get; set; } // Nullable

    [JsonPropertyName("delegate")]
    public bool Delegate { get; set; } // True for `yield*`
}