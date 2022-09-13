using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class AdditionExpression : ArithmeticExpression
{
    public AdditionExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
