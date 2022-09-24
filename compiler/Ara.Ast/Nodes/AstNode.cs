using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record AstNode(Node Node)
{
    public AstNode? _Parent { get; set; }
    
    public abstract List<AstNode> Children { get; }
    
    public T? NearestAncestorOrDefault<T>() where T : AstNode
    {
        var n = _Parent;
        while (true)
        {
            switch (n)
            {
                case null:
                    return null;
                case T node:
                    return node;
                default:
                    n = n._Parent;
                    break;
            }
        }
    }

    public T NearestAncestor<T>() where T : AstNode
    {
        var a = NearestAncestorOrDefault<T>();

        if (a is null)
            throw new Exception();

        return a;
    }
}
