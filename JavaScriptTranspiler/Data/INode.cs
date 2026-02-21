using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Declarations;
using JavaScriptTranspiler.Data.Expressions;
using JavaScriptTranspiler.Data.Patterns;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data;

// The Root ESTree Node
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ProgramNode), "Program")]
[JsonDerivedType(typeof(VariableDeclaration), "VariableDeclaration")]
[JsonDerivedType(typeof(VariableDeclarator), "VariableDeclarator")]
[JsonDerivedType(typeof(Identifier), "Identifier")]
[JsonDerivedType(typeof(BinaryExpression), "BinaryExpression")]
[JsonDerivedType(typeof(Literal), "Literal")]
[JsonDerivedType(typeof(ExpressionStatement), "ExpressionStatement")]
[JsonDerivedType(typeof(IfStatement), "IfStatement")]
[JsonDerivedType(typeof(SwitchStatement), "SwitchStatement")]
[JsonDerivedType(typeof(SwitchCase), "SwitchCase")]
[JsonDerivedType(typeof(WhileStatement), "WhileStatement")]
[JsonDerivedType(typeof(ForStatement), "ForStatement")]
[JsonDerivedType(typeof(FunctionDeclaration), "FunctionDeclaration")]
[JsonDerivedType(typeof(BlockStatement), "BlockStatement")]
[JsonDerivedType(typeof(ObjectPattern), "ObjectPattern")]
[JsonDerivedType(typeof(ArrayPattern), "ArrayPattern")]
[JsonDerivedType(typeof(RestElement), "RestElement")]
[JsonDerivedType(typeof(AssignmentPattern), "AssignmentPattern")]
[JsonDerivedType(typeof(MemberExpression), "MemberExpression")]
[JsonDerivedType(typeof(TemplateElement), "TemplateElement")]
[JsonDerivedType(typeof(Property), "Property")]
[JsonDerivedType(typeof(CatchClause), "CatchClause")]
[JsonDerivedType(typeof(ClassBody), "ClassBody")]
[JsonDerivedType(typeof(MethodDefinition), "MethodDefinition")]
[JsonDerivedType(typeof(PropertyDefinition), "PropertyDefinition")]
[JsonDerivedType(typeof(PrivateIdentifier), "PrivateIdentifier")]
[JsonDerivedType(typeof(ClassDeclaration), "ClassDeclaration")]
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
[JsonDerivedType(typeof(TemplateLiteral), "TemplateLiteral")]
[JsonDerivedType(typeof(TaggedTemplateExpression), "TaggedTemplateExpression")]
[JsonDerivedType(typeof(ArrayExpression), "ArrayExpression")]
[JsonDerivedType(typeof(SpreadElement), "SpreadElement")]
[JsonDerivedType(typeof(ObjectExpression), "ObjectExpression")]
[JsonDerivedType(typeof(LogicalExpression), "LogicalExpression")]
[JsonDerivedType(typeof(UnaryExpression), "UnaryExpression")]
[JsonDerivedType(typeof(UpdateExpression), "UpdateExpression")]
[JsonDerivedType(typeof(AssignmentExpression), "AssignmentExpression")]
[JsonDerivedType(typeof(SequenceExpression), "SequenceExpression")]
[JsonDerivedType(typeof(ArrowFunctionExpression), "ArrowFunctionExpression")]
[JsonDerivedType(typeof(YieldExpression), "YieldExpression")]
[JsonDerivedType(typeof(AwaitExpression), "AwaitExpression")]
[JsonDerivedType(typeof(FunctionExpression), "FunctionExpression")]
[JsonDerivedType(typeof(CallExpression), "CallExpression")]
[JsonDerivedType(typeof(ThisExpression), "ThisExpression")]
[JsonDerivedType(typeof(Super), "Super")]
[JsonDerivedType(typeof(MetaProperty), "MetaProperty")]
[JsonDerivedType(typeof(NewExpression), "NewExpression")]
public interface INode
{
    // The spec mandates a 'type' string
    [JsonPropertyName("type")] string Type { get; }

    [JsonPropertyName("start")] int Start { get; set; }

    [JsonPropertyName("end")] int End { get; set; }
}