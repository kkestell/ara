using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record IfElse(Node Node, Expression Predicate, Block Then, Block Else) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new() { Predicate, Then, Else };
}