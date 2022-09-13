using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class InequalityExpression : LogicalComparisonExpression
{
    public InequalityExpression(Node node, Expression left, Expression right) : base(node, left, right)
    {
    }
}
