using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Declarations;

namespace JavaScriptTranspiler.Data.Statements;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(VariableDeclaration), "VariableDeclaration")]
[JsonDerivedType(typeof(BlockStatement), "BlockStatement")]
[JsonDerivedType(typeof(IfStatement), "IfStatement")]
[JsonDerivedType(typeof(SwitchStatement), "SwitchStatement")]
[JsonDerivedType(typeof(WhileStatement), "WhileStatement")]
[JsonDerivedType(typeof(ForStatement), "ForStatement")]
[JsonDerivedType(typeof(FunctionDeclaration), "FunctionDeclaration")]
[JsonDerivedType(typeof(ExpressionStatement), "ExpressionStatement")]
[JsonDerivedType(typeof(EmptyStatement), "EmptyStatement")]
[JsonDerivedType(typeof(DoWhileStatement), "DoWhileStatement")]
[JsonDerivedType(typeof(LabeledStatement), "LabeledStatement")]
[JsonDerivedType(typeof(ContinueStatement), "ContinueStatement")]
[JsonDerivedType(typeof(BreakStatement), "BreakStatement")]
[JsonDerivedType(typeof(ForInStatement), "ForInStatement")]
[JsonDerivedType(typeof(ForOfStatement), "ForOfStatement")]
[JsonDerivedType(typeof(TryStatement), "TryStatement")]
[JsonDerivedType(typeof(ThrowStatement), "ThrowStatement")]
[JsonDerivedType(typeof(ReturnStatement), "ReturnStatement")]
[JsonDerivedType(typeof(DebuggerStatement), "DebuggerStatement")]
public interface IStatement : INode;