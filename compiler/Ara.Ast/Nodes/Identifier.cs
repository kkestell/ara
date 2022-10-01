using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Identifier(Node Node, string Value) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new ();
}