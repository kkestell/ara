using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Return(Node Node, Expression Expression) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Expression };
}
