using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class Super : IExpression
{
    public string Type => "Super";
    public int Start { get; set; }
    public int End { get; set; }
}