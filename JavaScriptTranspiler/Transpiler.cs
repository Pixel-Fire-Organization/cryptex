using System.Text.Json;
using JavaScriptTranspiler.Data;
using JavaScriptTranspiler.Exceptions;
using Jint;

namespace JavaScriptTranspiler;

public static class Transpiler
{
    private const string EXCEPTION_TOKEN = "__exception: ";
    
    public static void Transpile(string fileContents, out ProgramNode? program)
    {
        program = null;

        var engine = new Engine();

        var acornScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Dependencies", "acorn.js");
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

        var escapedJs = JsonSerializer.Serialize(fileContents);

        var json = engine.Execute($$"""
                                    const code = {{escapedJs}}; 

                                    try {
                                        const ast = acorn.parse(code, { 
                                            ecmaVersion: 2022,
                                            allowReturnOutsideFunction: true 
                                        });
                                        
                                        astJson = JSON.stringify(ast, (key, value) => {
                                           return typeof value === 'bigint' ? value.toString() : value;
                                        }, 2);
                                       
                                    } catch(e) {
                                        astJson = "{{EXCEPTION_TOKEN}}" + e.message;
                                    }
                                    """).GetValue("astJson").ToString();

        if (json.StartsWith(EXCEPTION_TOKEN))
            throw new JavaScriptASTParseException(json.Remove(0, EXCEPTION_TOKEN.Length));
        
        try
        {
            program = JsonSerializer.Deserialize<ProgramNode>(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            program = null;
        }
    }
}