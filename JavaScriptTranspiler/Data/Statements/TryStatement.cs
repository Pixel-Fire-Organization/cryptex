using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class TryStatement : IStatement
{
    public string Type => "TryStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("block")]
    public BlockStatement Block { get; set; }

    [JsonPropertyName("handler")]
    public CatchClause Handler { get; set; } // Nullable if there's only a finally block

    [JsonPropertyName("finalizer")]
    public BlockStatement Finalizer { get; set; } // Nullable
}