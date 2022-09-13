using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public class Bool : Atom
{
    public Bool(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
