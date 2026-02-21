using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Declarations;

public class VariableDeclaration : IDeclaration
{
    public string Type => "VariableDeclaration";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("declarations")]
    public List<VariableDeclarator> Declarations { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; } // "var", "let", or "const"
}