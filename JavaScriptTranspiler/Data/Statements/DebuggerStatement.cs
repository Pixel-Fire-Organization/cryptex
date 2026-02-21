namespace JavaScriptTranspiler.Data.Statements;

public class DebuggerStatement : IStatement
{
    public string Type => "DebuggerStatement";
    public int Start { get; set; }
    public int End { get; set; }
}