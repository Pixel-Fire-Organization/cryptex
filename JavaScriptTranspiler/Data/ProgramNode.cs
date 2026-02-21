using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class ProgramNode : INode
{
    public string Type => "Program";
    public int Start { get; set; }
    public int End { get; set; }
    
    [JsonPropertyName("body")]
    public List<INode> Body { get; set; } 

    [JsonPropertyName("sourceType")]
    public string SourceType { get; set; }
}