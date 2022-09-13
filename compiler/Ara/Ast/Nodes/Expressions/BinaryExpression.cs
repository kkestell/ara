using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public abstract class ArithmeticExpression : BinaryExpression
{
    protected ArithmeticExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}

public abstract class LogicalComparisonExpression : BinaryExpression
{
    protected LogicalComparisonExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}

public abstract class BinaryExpression : Expression
{
    protected BinaryExpression(Node node, Expression left, Expression right) : base(node)
    {
        Left = left;
        Right = right;
    }
    
    public Expression Left { get; }
    
    public Expression Right { get; }
}
