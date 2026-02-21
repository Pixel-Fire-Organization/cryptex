using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Statements;

public class SwitchCase : INode
{
    public string Type => "SwitchCase";
    public int Start { get; set; }
    public int End { get; set; }

    // Test is null for the 'default:' case
    [JsonPropertyName("test")] public IExpression Test { get; set; }

    [JsonPropertyName("consequent")] public List<IStatement> Consequent { get; set; }
}