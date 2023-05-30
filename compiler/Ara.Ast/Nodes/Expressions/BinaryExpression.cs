#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Types;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions;

public record BinaryExpression(IParseNode Node, Expression Left, Expression Right, BinaryOperator Op) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Left, Right };

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
    }
}

