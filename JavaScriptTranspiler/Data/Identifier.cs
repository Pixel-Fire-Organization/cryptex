namespace JavaScriptTranspiler.Data;

public class Identifier(string name, SourceLocation loc) : IPattern, IExpression
{
    public string Type => "Identifier";
    public long Start { get; }
    public long End { get; }
    //public SourceLocation Loc { get; } = loc;
    public string Name { get; } = name;
}