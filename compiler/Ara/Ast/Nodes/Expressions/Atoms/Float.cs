using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class Float : Atom
{
    public Float(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
