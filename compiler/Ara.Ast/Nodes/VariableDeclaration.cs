using Ara.Ast.Semantics;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, TypeRef TypeRef, Identifier Name, Expression? Expression) : Statement(Node)
{
    public readonly Type Type = Type.Parse(TypeRef);
}
