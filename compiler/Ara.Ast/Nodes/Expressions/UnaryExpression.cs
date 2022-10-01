using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions;

public record UnaryExpression(Node Node, Expression Right, UnaryOperator Op) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new() { Right };

    public override Type Type
    {
        get => Right.Type;
        set => throw new NotImplementedException();
    }
}
