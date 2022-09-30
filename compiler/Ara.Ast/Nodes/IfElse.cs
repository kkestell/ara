using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record IfElse(Node Node, Expression Predicate, Block Then, Block Else) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Predicate, Then, Else };
}