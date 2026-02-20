using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data;

public interface INode
{
    [JsonPropertyName("type")] string Type { get; }
    //[JsonPropertyName("loc")] SourceLocation Loc { get; }
    [JsonPropertyName("start")] long Start { get; }
    [JsonPropertyName("end")] long End { get; }
}