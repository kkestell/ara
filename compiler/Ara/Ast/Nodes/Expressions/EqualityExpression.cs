using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class EqualityExpression : LogicalComparisonExpression
{
    public EqualityExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
