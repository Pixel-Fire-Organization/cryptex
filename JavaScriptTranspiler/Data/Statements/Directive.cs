using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Statements;

public class ExpressionStatement : IStatement
{
    public string Type => "ExpressionStatement";
    public int Start { get; set; }
    public int End { get; set; }

    [JsonPropertyName("expression")]
    public IExpression Expression { get; set; }

    // If this statement is a directive (like "use strict"), Acorn populates this field.
    // For normal expressions (like `a = 5;`), this will simply be null.
    [JsonPropertyName("directive")]
    public string Directive { get; set; }
}