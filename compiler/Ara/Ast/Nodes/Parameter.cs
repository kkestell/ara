using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class Parameter : AstNode
{
    public Parameter(Node node, Identifier name, Type_ type) : base(node)
    {
        Name = name;
        Type = type;
    }
    
    public Identifier Name { get; }
    
    public Type_ Type { get; }
}
