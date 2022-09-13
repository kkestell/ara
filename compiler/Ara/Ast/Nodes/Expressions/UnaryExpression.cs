using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public abstract class UnaryExpression : Expression
{
    protected UnaryExpression(Node node, Expression right) : base(node)
    {
        Right = right;
    }
    
    public Expression Right { get; }
}
