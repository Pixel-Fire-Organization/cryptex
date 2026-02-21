using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class IfStatement : IStatement
{
    public string Type => "IfStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("test")]
    public IExpression Test { get; set; }

    [JsonPropertyName("consequent")]
    public IStatement Consequent { get; set; }

    // Alternate can be null (no 'else'), an IfStatement (for 'else if'), or a BlockStatement
    [JsonPropertyName("alternate")]
    public IStatement Alternate { get; set; } 
}