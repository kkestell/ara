using Ara.Ast.Semantics;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, string Name, TypeRef TypeRef) : AstNode(Node)
{
    public Type Type => Type.Parse(TypeRef);
}
