#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions;

public record UnaryExpression(IParseNode Node, Expression Right, UnaryOperator Op) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Right };

    public override Type Type => Right.Type;
}
