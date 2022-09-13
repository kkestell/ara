using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class Integer : Atom
{
    public Integer(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
