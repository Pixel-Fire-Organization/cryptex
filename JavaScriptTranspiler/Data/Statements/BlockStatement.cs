using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class BlockStatement : IStatement
{
    public string Type => "BlockStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("body")]
    public List<IStatement> Body { get; set; }
}