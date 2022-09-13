using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class MultiplicationExpression : ArithmeticExpression
{
    public MultiplicationExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
