using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Argument(IParseNode Node, string Name, Expression Expression) : AstNode(Node)
{
    readonly AstNode[] children = { Expression };

    public override IEnumerable<AstNode> Children => children;
}
