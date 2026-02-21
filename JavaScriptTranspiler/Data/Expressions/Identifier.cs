using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Patterns;

namespace JavaScriptTranspiler.Data.Expressions;

public class Identifier : IExpression, IPattern
{
    public string Type => "Identifier";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}