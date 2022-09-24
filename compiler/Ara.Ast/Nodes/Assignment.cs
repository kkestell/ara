using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Assignment(Node Node, string Name, Expression Expression) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Expression };
}
