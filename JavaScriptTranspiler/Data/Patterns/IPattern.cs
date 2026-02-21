using System.Text.Json.Serialization;
using JavaScriptTranspiler.Data.Declarations;
using JavaScriptTranspiler.Data.Expressions;

namespace JavaScriptTranspiler.Data.Patterns;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Identifier), "Identifier")]
[JsonDerivedType(typeof(ObjectPattern), "ObjectPattern")]
[JsonDerivedType(typeof(ArrayPattern), "ArrayPattern")]
[JsonDerivedType(typeof(RestElement), "RestElement")]
[JsonDerivedType(typeof(AssignmentPattern), "AssignmentPattern")]
[JsonDerivedType(typeof(MemberExpression), "MemberExpression")]
public interface IPattern : INode;