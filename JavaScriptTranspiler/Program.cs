using System.Text.Json;

using JavaScriptTranspiler.Data;

namespace JavaScriptTranspiler;

public class Program
{
    public static void Main(string[] args)
    {
        Transpiler.Transpile(
            """var a = 'abc' + 25; var b = a + 0.3;""", out Data.Program? program, out Function? function);

        var json = JsonSerializer.Serialize(program);

        Console.WriteLine(json);
        Console.ReadLine();
    }
}