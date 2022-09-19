using Ara.Parsing;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.Ast.Nodes;

public abstract record AstNode(Node Node)
{
    public AstNode? _Parent { get; set; }
    
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

    public Type? ResolveVariableReference(string name)
    {
        var blk = NearestAncestor<Block>();
        while (true)
        {
            if (blk is null)
                return null;

            if (blk.Scope.ContainsKey(name))
                return blk.Scope[name];
            
            blk = blk.NearestAncestorOrDefault<Block>();
        }
    }
}
