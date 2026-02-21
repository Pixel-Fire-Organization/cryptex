using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Patterns;

public class ObjectPattern : IPattern
{
    public string Type => "ObjectPattern";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("properties")]
    public List<INode> Properties { get; set; } // Property or RestElement
}