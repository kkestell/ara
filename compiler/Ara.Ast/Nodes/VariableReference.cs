using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableReference(Node Node, string Name) : Atom(Node)
{
    public AstNode? ResolveReference(string name)
    {
        var b = NearestAncestor<Block>();
        while (true)
        {
            if (b is null)
                return null;

            if (b.Scope.ContainsKey(name))
                return b.Scope[name];
            
            b = b.NearestAncestorOrDefault<Block>();
        }
    }
}
