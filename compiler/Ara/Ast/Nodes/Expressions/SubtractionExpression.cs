using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class SubtractionExpression : ArithmeticExpression
{
    public SubtractionExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
