using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Ast.Types;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Statements;

public record For(Node Node, string Counter, Expression Start, Expression End, Block Block) : Statement(Node), ITyped
{
    public override List<AstNode> Children { get; } = new() { Start, End, Block };

    public Type Type
    {
        get => new IntegerType();
        set => throw new NotSupportedException();
    }
}
