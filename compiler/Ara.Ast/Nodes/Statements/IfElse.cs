using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Statements;

public record IfElse(IParseNode Node, Expression Predicate, Block Then, Block Else) : Statement(Node)
{
    readonly AstNode[] children = {  Predicate, Then, Else  };

    public override IEnumerable<AstNode> Children => children;
}