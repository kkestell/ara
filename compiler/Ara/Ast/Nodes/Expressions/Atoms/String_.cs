using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class String_ : Atom
{
    public String_(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
