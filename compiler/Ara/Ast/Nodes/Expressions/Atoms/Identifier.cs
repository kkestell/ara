using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class Identifier : Atom
{
    public Identifier(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
