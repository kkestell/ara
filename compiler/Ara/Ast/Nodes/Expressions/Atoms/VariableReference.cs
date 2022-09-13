using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class VariableReference : Atom
{
    public VariableReference(Node node, Identifier name) : base(node)
    {
        Name = name;
    }
    
    public Identifier Name { get; }
}
