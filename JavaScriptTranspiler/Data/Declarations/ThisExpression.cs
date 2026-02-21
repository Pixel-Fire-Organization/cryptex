using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class ThisExpression : IExpression
{
    public string Type => "ThisExpression";
    public int Start { get; set; }
    public int End { get; set; }
}