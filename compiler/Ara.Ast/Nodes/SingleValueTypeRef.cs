using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record SingleValueTypeRef(Node Node, string Name) : TypeRef(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { };
}