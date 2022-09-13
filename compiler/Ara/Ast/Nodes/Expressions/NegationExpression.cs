using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class NegationExpression : UnaryExpression
{
    public NegationExpression(Node node, Expression right) : base(node, right)
    {
    }
}
