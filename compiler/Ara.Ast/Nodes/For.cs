using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record For(Node Node, string Counter, Expression Start, Expression End, Block Block) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Start, End, Block };
}
