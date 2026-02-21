using System.Text.Json;
using JavaScriptTranspiler.Data;
using JavaScriptTranspiler.Data.Declarations;

namespace JavaScriptTranspiler;

public class Program
{
    public static void Main(string[] args)
    {
        string fileContents =
            File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "functionEcma2022.js"));

        Transpiler.Transpile(fileContents, out ProgramNode? program);

        var json = JsonSerializer.Serialize(program);

        Console.WriteLine(json);
    }
}