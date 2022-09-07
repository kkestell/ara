using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public record SubtractionExpression : BinaryExpression
{
    public SubtractionExpression(Node Node, Expression Left, Expression Right) : base(Node, Left, Right)
    {
    }
}
