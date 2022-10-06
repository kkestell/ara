using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Ast.Types;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Statements;

public record For(IParseNode Node, string Counter, Expression Start, Expression End, Block Block) : Statement(Node), ITyped
{
    public override IEnumerable<AstNode> Children { get; } = new AstNode[] { Start, End, Block };

    public Type Type => new IntegerType();
}
