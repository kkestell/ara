using Ara.Ast.Errors;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableReference(Node Node, string Name) : Atom(Node)
{
    public ITyped ResolveReference(string name)
    {
        var b = NearestAncestor<Block>();
        while (true)
        {
            if (b is null)
                throw new ReferenceException(this);

            if (b.Scope.ContainsKey(name))
            {
                var foo = b.Scope[name];
                if (foo is not ITyped i)
                    throw new Exception();

                return i;
            }

            b = b.NearestAncestorOrDefault<Block>();
        }
    }

    public override Type Type
    { 
        get => ResolveReference(Name).Type;
        set => throw new NotImplementedException();
    }

}
