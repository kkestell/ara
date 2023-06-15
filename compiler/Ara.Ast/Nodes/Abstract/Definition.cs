using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Abstract;

public abstract record Definition(IParseNode Node, string Name) : AstNode(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode>();
}