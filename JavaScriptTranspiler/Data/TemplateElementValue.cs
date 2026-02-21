using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class TemplateElementValue
{
    [JsonPropertyName("raw")]
    public string Raw { get; set; }

    [JsonPropertyName("cooked")]
    public string Cooked { get; set; }
}