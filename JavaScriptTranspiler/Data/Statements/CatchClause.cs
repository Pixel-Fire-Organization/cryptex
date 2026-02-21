using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Patterns;

namespace JavaScriptTranspiler.Data.Statements;

public class CatchClause : INode
{
    public string Type => "CatchClause";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("param")]
    public IPattern Param { get; set; } // Nullable in modern JS: `catch { }`

    [JsonPropertyName("body")]
    public BlockStatement Body { get; set; }
}