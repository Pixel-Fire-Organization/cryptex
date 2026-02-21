using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Declarations;

public class FunctionDeclaration : IDeclaration
{
    public string Type => "FunctionDeclaration";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("id")]
    public Identifier Id { get; set; }

    [JsonPropertyName("params")]
    public List<IPattern> Params { get; set; }

    [JsonPropertyName("body")]
    public BlockStatement Body { get; set; }
}