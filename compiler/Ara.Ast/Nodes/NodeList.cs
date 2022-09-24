using System.Collections;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record NodeList<T>(Node Node, List<T> Nodes) : AstNode(Node), IEnumerable where T : AstNode
{
    public override List<AstNode> Children { get; } = Nodes.Select(x => x as AstNode).ToList();
    
    public IEnumerator GetEnumerator()
    {
        return Children.GetEnumerator();
    }
}
