using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Expressions;

public class TemplateLiteral : IExpression
{
    public string Type => "TemplateLiteral";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("quasis")]
    public List<TemplateElement> Quasis { get; set; }

    [JsonPropertyName("expressions")]
    public List<IExpression> Expressions { get; set; }
}