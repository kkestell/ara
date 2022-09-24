using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record UnaryExpression(Node Node, Expression Right, UnaryOperator Op) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Right };
}
