using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Identifier(IParseNode Node, string Value) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new ();
}