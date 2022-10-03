using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Statements;

public record Return(IParseNode Node, Expression Expression) : Statement(Node)
{
    readonly AstNode[] children = { Expression };

    public override IEnumerable<AstNode> Children => children;
}
