using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, TypeRef TypeRef, string Name, Expression? Expression) : Statement(Node)
{
    public readonly Type Type = Type.Parse(TypeRef);
}
