#region

using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes.Abstract;

public abstract record AstNode(IParseNode Node)
{
    public AstNode? Parent { get; set; }
    
    public abstract IEnumerable<AstNode> Children { get; }

    public ITyped? FindDeclaration(string name)
    {
        var n = this;
        while (true)
        {
            switch (n)
            {
                case null:
                    return null;
                case Block b:
                    if (b.Scope.ContainsKey(name))
                        return b.Scope[name];
                    else
                        n = n.Parent;
                    break;
                default:
                    n = n.Parent;
                    break;
            }
        }
    }
    
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
}
