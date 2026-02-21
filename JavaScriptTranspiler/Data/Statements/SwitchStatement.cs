using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class SwitchStatement : IStatement
{
    public string Type => "SwitchStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("discriminant")]
    public IExpression Discriminant { get; set; }

    [JsonPropertyName("cases")]
    public List<SwitchCase> Cases { get; set; }
}