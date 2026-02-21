using System.Text.Json;
using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class Literal : IExpression
{
    public string Type => "Literal";
    public int Start { get; set; }
    public int End { get; set; }

    // ESTree spec says value is string | boolean | null | number | RegExp.
    // JsonElement perfectly represents this union type in C#.
    [JsonPropertyName("value")]
    public JsonElement Value { get; set; }

    [JsonPropertyName("raw")]
    public string Raw { get; set; }
    
    // This will be populated by Acorn if the literal is a regular expression, 
    // otherwise it will simply be null.
    [JsonPropertyName("regex")]
    public RegexData Regex { get; set; }
}