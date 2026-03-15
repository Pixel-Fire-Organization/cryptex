namespace Cryptex;

public static class PrintingDelegates
{
    public static Action<string> WriteMessage { get; set; } = s =>
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[MSG] {s}");
        Console.ForegroundColor = fg;
    };
    public static Action<string> WriteWarning { get; set; } = s =>
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WRN] {s}");
        Console.ForegroundColor = fg;
    };
    public static Action<string> WriteError { get; set; } = s =>
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERR] {s}");
        Console.ForegroundColor = fg;
    };
    
    public static Action<Exception> WriteException { get; set; } = ex =>
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[EXC] {ex}");
        Console.ForegroundColor = fg;
    };
}
