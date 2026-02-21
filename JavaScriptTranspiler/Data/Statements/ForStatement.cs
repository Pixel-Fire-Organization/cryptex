using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class ForStatement : IStatement
{
    public string Type => "ForStatement";
    public int Start { get; set; }
    public int End { get; set; }

    // Init can be a VariableDeclaration (let i = 0), an Expression (i = 0), or null
    [JsonPropertyName("init")]
    public INode Init { get; set; } 

    [JsonPropertyName("test")]
    public IExpression Test { get; set; } // Can be null

    [JsonPropertyName("update")]
    public IExpression Update { get; set; } // Can be null

    [JsonPropertyName("body")]
    public IStatement Body { get; set; }
}