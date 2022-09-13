using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract class AstNode
{
    protected AstNode(Node node, AstNode? parent = null)
    {
        Node = node;
        _Parent = parent;
    }
    
    public Node Node { get; }
    
    public AstNode? _Parent { get; set; }

    public T? NearestAncestor<T>() where T : AstNode
    {
        var n = this;

        while (true)
        {
            if (n is null)
                return null;

            if (n is T node)
                return node;

            n = n._Parent;
        }
    }
}
