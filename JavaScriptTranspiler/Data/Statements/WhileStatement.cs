using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class WhileStatement : IStatement
{
    public string Type => "WhileStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("test")]
    public IExpression Test { get; set; }

    [JsonPropertyName("body")]
    public IStatement Body { get; set; }
}