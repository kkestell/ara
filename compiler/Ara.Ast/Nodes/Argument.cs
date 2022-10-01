using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Argument(Node Node, string Name, Expression Expression) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new() { Expression };
}
