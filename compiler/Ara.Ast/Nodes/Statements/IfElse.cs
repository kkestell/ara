using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Statements;

public record IfElse(IParseNode Node, Expression Predicate, Block Then, Block Else) : Statement(Node)
{
    public override List<AstNode> Children { get; } = new() { Predicate, Then, Else };
}