using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public class RegexData
{
    [JsonPropertyName("pattern")]
    public string Pattern { get; set; }

    [JsonPropertyName("flags")]
    public string Flags { get; set; }
}