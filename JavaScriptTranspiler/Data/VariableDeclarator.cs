using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;

namespace JavaScriptTranspiler.Data;

public class VariableDeclarator : INode
{
    public string Type => "VariableDeclarator";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("id")]
    public IPattern Id { get; set; } // Usually an Identifier

    [JsonPropertyName("init")]
    public IExpression Init { get; set; } // Can be null if just 'var a;'
}