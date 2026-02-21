using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Statements;

public class DoWhileStatement : IStatement
{
    public string Type => "DoWhileStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("body")]
    public IStatement Body { get; set; }

    [JsonPropertyName("test")]
    public IExpression Test { get; set; }
}