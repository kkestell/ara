using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ArrayAssignment(Node Node, string Name, Expression Index, Expression Expression) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Expression };
}