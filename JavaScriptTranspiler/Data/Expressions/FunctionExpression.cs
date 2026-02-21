using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Expressions;

public class FunctionExpression : IExpression
{
    public string Type => "FunctionExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("id")]
    public Identifier Id { get; set; } // Nullable (anonymous functions don't have names)

    [JsonPropertyName("params")]
    public List<IPattern> Params { get; set; }

    [JsonPropertyName("body")]
    public BlockStatement Body { get; set; }

    [JsonPropertyName("generator")]
    public bool Generator { get; set; } // True if `function*()`

    [JsonPropertyName("async")]
    public bool Async { get; set; } // True if `async function()`
}