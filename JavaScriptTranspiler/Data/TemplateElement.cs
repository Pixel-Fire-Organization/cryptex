using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class TemplateElement : INode
{
    public string Type => "TemplateElement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("tail")]
    public bool Tail { get; set; }

    [JsonPropertyName("value")]
    public TemplateElementValue Value { get; set; }
}