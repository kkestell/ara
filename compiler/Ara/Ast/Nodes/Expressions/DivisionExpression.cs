using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class DivisionExpression : ArithmeticExpression
{
    public DivisionExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
