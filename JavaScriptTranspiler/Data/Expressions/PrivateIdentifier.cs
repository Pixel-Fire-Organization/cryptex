using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Expressions;

public class PrivateIdentifier : IExpression
{
    public string Type => "PrivateIdentifier";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}