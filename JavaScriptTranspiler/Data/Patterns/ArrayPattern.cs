using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Patterns;

public class ArrayPattern : IPattern
{
    public string Type => "ArrayPattern";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("elements")]
    public List<IPattern> Elements { get; set; } // Can contain nulls
}