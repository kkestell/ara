using Ara.Ast.Errors;
using Ara.Ast.Types;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions;

public record BinaryExpression(Node Node, Expression Left, Expression Right, BinaryOperator Op) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new() { Left, Right };

    public override Type Type
    {
        get
        {
            if (Left.Type is null || Right.Type is null)
                throw new BinaryExpressionTypeException(this);

            if (!Left.Type.Equals(Right.Type))
                throw new BinaryExpressionTypeException(this);

            return Op is BinaryOperator.Equality or BinaryOperator.Inequality ? new BooleanType() : Left.Type;   
        }
        
        set => throw new NotImplementedException();
    }
}

