using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Abstract;

public abstract record AstNode(IParseNode Node)
{
    public AstNode? Parent { get; set; }
    
    public abstract IEnumerable<AstNode> Children { get; }
    
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
        return NearestAncestorOrDefault<T>() ?? throw new Exception();
    }

    public IEnumerable<T> Descendants<T>() where T : AstNode
    {
        var nodes = new List<T>();

        foreach (var c in Children)
        {
            nodes.AddRange(c.Descendants<T>());
        }
        
        nodes.AddRange(Children.OfType<T>());

        return nodes;
    }
}
