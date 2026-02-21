using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Patterns;

public class AssignmentPattern : IPattern
{
    public string Type => "AssignmentPattern";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("left")]
    public IPattern Left { get; set; }

    [JsonPropertyName("right")]
    public IExpression Right { get; set; }
}