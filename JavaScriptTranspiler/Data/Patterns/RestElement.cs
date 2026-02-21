using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Patterns;

public class RestElement : IPattern
{
    public string Type => "RestElement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("argument")]
    public IPattern Argument { get; set; }
}