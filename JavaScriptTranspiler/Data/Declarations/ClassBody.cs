using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Declarations;

public class ClassBody : INode
{
    public string Type => "ClassBody";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("body")]
    public List<INode> Body { get; set; } // MethodDefinition or PropertyDefinition
}