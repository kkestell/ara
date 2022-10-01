using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Types;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public record For(Node Node, string Counter, Expression Start, Expression End, Block Block) : Statement(Node), ITyped
{
    public override List<AstNode> Children { get; } = new() { Start, End, Block };

    public Type Type
    {
        get => new IntegerType();
        set => throw new NotSupportedException();
    }
}
