using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record Assignment(Node Node, string Name, Expression Expression) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new() { Expression };
}
