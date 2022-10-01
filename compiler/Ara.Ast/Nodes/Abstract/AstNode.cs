using Ara.Parsing;

namespace Ara.Ast.Nodes.Abstract;

public abstract record AstNode(Node Node)
{
    public AstNode? Parent { get; set; }
    
    public abstract List<AstNode> Children { get; }
    
    public T? NearestAncestorOrDefault<T>() where T : AstNode
    {
        var n = Parent;
        while (true)
        {
            switch (n)
            {
                case null:
                    return null;
                case T node:
                    return node;
                default:
                    n = n.Parent;
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
