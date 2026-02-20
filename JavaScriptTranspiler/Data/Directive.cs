using System.Text.Json.Serialization;

using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data;

public class Directive : ExpressionStatement
{
    [JsonPropertyName("expression")]
    public new Literal Expression { get; }

    [JsonPropertyName("directive")]
    public string _Directive { get; }
}