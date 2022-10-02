using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Argument(IParseNode Node, string Name, Expression Expression) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new() { Expression };
}
