using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class BreakStatement : IStatement
{
    public string Type => "BreakStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("label")]
    public Identifier Label { get; set; } // Nullable
}