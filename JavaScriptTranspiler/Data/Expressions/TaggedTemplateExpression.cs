using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Expressions;

public class TaggedTemplateExpression : IExpression
{
    public string Type => "TaggedTemplateExpression";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("tag")]
    public IExpression Tag { get; set; }

    [JsonPropertyName("quasi")]
    public TemplateLiteral Quasi { get; set; }
}