using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record BinaryExpression(Node Node, Expression Left, Expression Right, BinaryOperator Op) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Left, Right };

    public override Type Type
    {
        get => Op is BinaryOperator.Equality or BinaryOperator.Inequality ? new BooleanType() : Left.Type;
        set => throw new NotImplementedException();
    }
}