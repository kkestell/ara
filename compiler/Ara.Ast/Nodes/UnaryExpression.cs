using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record UnaryExpression(Node Node, Expression Right, UnaryOperator Op) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Right };

    public override Type Type
    {
        get => Right.Type;
        set => throw new NotImplementedException();
    }
}
