using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class ForInStatement : IStatement
{
    public string Type => "ForInStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("left")]
    public INode Left { get; set; } // VariableDeclaration or IPattern

    [JsonPropertyName("right")]
    public IExpression Right { get; set; }

    [JsonPropertyName("body")]
    public IStatement Body { get; set; }
}