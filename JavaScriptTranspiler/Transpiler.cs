using System.Text.Json;

using JavaScriptTranspiler.Data;

using Jint;

namespace JavaScriptTranspiler;

public static class Transpiler
{
    public static void Transpile(string fileContents, out Data.Program? program, out Function? function)
    {
        function = null;
        program = null;

        fileContents = fileContents.ReplaceLineEndings("");
        
        var engine = new Engine();

        var acornScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "acorn.js");
        if (!File.Exists(acornScriptPath))
        {
            Console.WriteLine("Acorn.js file not found.");

            return;
        }

        var acornCode = File.ReadAllText(acornScriptPath);
        // Wrap the library code to assign it to a global variable
        acornCode = $"var acorn = (function() {{ {acornCode}; return this.acorn || module.exports; }})();";
        engine.Execute(acornCode); // Preload Acorn library
        engine.SetValue("astJson", "");

        var json = engine.Execute($$"""
                                    const code = "{{fileContents}}";
                                    try {
                                        const ast = acorn.parse(code, { ecmaVersion: 6 });
                                        astJson = JSON.stringify(ast);
                                    } catch(e) {
                                        astJson = "__exception: " + e.message;   
                                    }
                                    """).GetValue("astJson").ToString();

        program = JsonSerializer.Deserialize<Data.Program>(json);
        function = JsonSerializer.Deserialize<Function>(json);
    }

    public static object GetTypeFromINode(INode node)
    {
        switch (node.Type)
        {
            case "Identifier":
                return (Identifier) node;

            case "Literal":
                return (Literal) node;

            case "Program":
                return (Data.Program) node;

            default:
            case "" or null:
                return node;
        }

        return node;
    }
}