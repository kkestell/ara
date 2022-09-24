using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record If(Node Node, Expression Predicate, Block Then) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Predicate, Then };
}