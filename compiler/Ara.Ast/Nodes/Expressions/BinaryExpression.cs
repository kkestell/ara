using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Types;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions;

public record BinaryExpression(IParseNode Node, Expression Left, Expression Right, BinaryOperator Op) : Expression(Node)
{
    readonly AstNode[] children = {  Left, Right  };

    public override IEnumerable<AstNode> Children => children;

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

