using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class Type_ : AstNode
{
    public Type_(Node node, string value) : base(node)
    {
        Value = value;
    }
    
    public string Value { get; }
}
