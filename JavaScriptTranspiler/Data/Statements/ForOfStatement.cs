using System.Text.Json.Serialization;

namespace JavaScriptTranspiler.Data.Statements;

public class ForOfStatement : ForInStatement // Same structure in ESTree, different type
{
    public new string Type => "ForOfStatement";
    
    [JsonPropertyName("await")]
    public bool Await { get; set; } // For `for await (let x of y)`
}