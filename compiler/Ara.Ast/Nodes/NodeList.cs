using System.Collections;
using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record NodeList<T>(IParseNode Node, List<T> Nodes) : AstNode(Node), IEnumerable where T : AstNode
{
    public override IEnumerable<AstNode> Children { get; } = Nodes.Select(x => x as AstNode).ToList();
    
    public IEnumerator GetEnumerator() => Children.GetEnumerator();
}
