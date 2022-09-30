using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ArrayTypeRef(Node Node, TypeRef Type, IntegerValue Size) : TypeRef(Node)
{
    public override List<AstNode> Children { get; } = new() { Type, Size };
}