using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Declarations;

public class NewExpression : CallExpression // Structurally identical in ESTree
{
    public new string Type => "NewExpression";
}