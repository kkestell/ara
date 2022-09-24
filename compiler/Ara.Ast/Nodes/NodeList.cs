using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record NodeList<T>(Node Node, List<T> Nodes) : AstNode(Node) where T : AstNode
{
    public override List<AstNode> Children { get; } = Nodes.Select(x => x as AstNode).ToList();
}
