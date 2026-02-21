using JavaScriptTranspiler.Data;
using JavaScriptTranspiler.Data.Declarations;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;
using JavaScriptTranspiler.Data.Statements;
using JavaScriptTranspiler.Exceptions;

namespace JavaScriptTranspiler.Tests;

public class TranspilerStressTests
{
    [Theory]
    [InlineData("Files/allEcma2022.js")]
    [InlineData("Files/functionEcma2022.js")]
    public void Transpile_ValidScripts_ReturnsPopulatedProgramNode(string fileName)
    {
        string fileContents = File.ReadAllText(fileName);
        
        Transpiler.Transpile(fileContents, out ProgramNode? program);
        
        Assert.NotNull(program);
        Assert.Equal("Program", program.Type);
        Assert.NotEmpty(program.Body);
    }

    [Fact]
    public void Transpile_AllScript_ContainsSpecificNodes()
    {
        string fileContents = File.ReadAllText("Files/allEcma2022.js");
        Transpiler.Transpile(fileContents, out ProgramNode? program);
        Assert.NotNull(program);
        
        var hasClass = program.Body.Any(n => n is ClassDeclaration);
        Assert.True(hasClass, "The transpiler failed to identify the ClassDeclaration in the stress test.");
        
        var hasTry = program.Body.Any(n => n is TryStatement);
        Assert.True(hasTry, "The transpiler failed to identify the TryStatement.");
    }

    [Fact]
    public void Transpile_FunctionScript_VerifyComplexParameters()
    {
        string fileContents = File.ReadAllText("Files/functionEcma2022.js");
        Transpiler.Transpile(fileContents, out ProgramNode? program);
        Assert.NotNull(program);
        
        var returnStmt = program.Body.OfType<ReturnStatement>().FirstOrDefault();
        Assert.NotNull(returnStmt);
        
        var funcExpr = returnStmt.Argument as FunctionExpression;
        Assert.NotNull(funcExpr);
        Assert.Equal("masterFunction", funcExpr.Id.Name);
        
        Assert.IsType<ObjectPattern>(funcExpr.Params[0]);
    }

    [Fact]
    public void Transpile_InvalidSyntax_ThrowsException()
    {
        string brokenCode = "var a = ;";
        
        ProgramNode? program = null;
        Assert.Throws<JavaScriptASTParseException>(() => Transpiler.Transpile(brokenCode, out program));

        Assert.Null(program);
    }
}