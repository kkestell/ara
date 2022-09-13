using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class LogicalNegationExpression : UnaryExpression
{
    public LogicalNegationExpression(Node node, Expression right) : base(node, right)
    {
    }
}
