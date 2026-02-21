using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Declarations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(VariableDeclaration), "VariableDeclaration")]
[JsonDerivedType(typeof(FunctionDeclaration), "FunctionDeclaration")]
[JsonDerivedType(typeof(ClassDeclaration), "ClassDeclaration")]
public interface IDeclaration : IStatement;