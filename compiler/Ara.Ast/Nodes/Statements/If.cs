﻿using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Statements;

public record If(IParseNode Node, Expression Predicate, Block Then) : Statement(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new AstNode[] { Predicate, Then };
}