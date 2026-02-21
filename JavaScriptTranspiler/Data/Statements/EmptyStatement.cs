namespace JavaScriptTranspiler.Data.Statements;

public class EmptyStatement : IStatement
{
    public string Type => "EmptyStatement";
    public int Start { get; set; }
    public int End { get; set; }
}