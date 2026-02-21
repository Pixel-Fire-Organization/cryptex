using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class ClassDeclaration : IDeclaration
{
    public string Type => "ClassDeclaration";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("id")]
    public Identifier Id { get; set; } // Nullable if exported anonymously

    [JsonPropertyName("superClass")]
    public IExpression SuperClass { get; set; } // Nullable

    [JsonPropertyName("body")]
    public ClassBody Body { get; set; }
}