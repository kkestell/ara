using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record For(Node Node, string Counter, Expression Start, Expression End, Block Block) : Statement(Node), ITyped
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Start, End, Block };

    public Type Type
    {
        get => new IntegerType();
        set => throw new NotSupportedException();
    }
}
