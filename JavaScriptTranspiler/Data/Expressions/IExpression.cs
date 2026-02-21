using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Declarations;
using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data.Expressions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Literal), "Literal")]
[JsonDerivedType(typeof(Identifier), "Identifier")]
[JsonDerivedType(typeof(BinaryExpression), "BinaryExpression")]
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
[JsonDerivedType(typeof(MemberExpression), "MemberExpression")]
[JsonDerivedType(typeof(Super), "Super")]
[JsonDerivedType(typeof(MetaProperty), "MetaProperty")]
[JsonDerivedType(typeof(NewExpression), "NewExpression")]
[JsonDerivedType(typeof(ConditionalExpression), "ConditionalExpression")]
[JsonDerivedType(typeof(PrivateIdentifier), "PrivateIdentifier")]
public interface IExpression : INode;