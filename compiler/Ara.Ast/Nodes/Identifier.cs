using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Identifier(IParseNode Node, string Value) : AstNode(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode>();
}