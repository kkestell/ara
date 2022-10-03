using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions;

public record UnaryExpression(IParseNode Node, Expression Right, UnaryOperator Op) : Expression(Node)
{
    readonly AstNode[] children = { Right };

    public override IEnumerable<AstNode> Children => children;

    public override Type Type
    {
        get => Right.Type;
        set => throw new NotImplementedException();
    }
}
