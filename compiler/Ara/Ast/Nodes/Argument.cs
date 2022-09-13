using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class Argument : AstNode
{
    public Argument(Node node, Identifier name, Expression expression) : base(node)
    {
        Name = name;
        Expression = expression;
    }
    
    public Identifier Name { get; }
    
    public Expression Expression { get; }
}
