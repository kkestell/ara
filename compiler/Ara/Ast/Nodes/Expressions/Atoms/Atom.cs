using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public abstract class Atom : Expression
{
    protected Atom(Node node) : base(node)
    {
    }
}
