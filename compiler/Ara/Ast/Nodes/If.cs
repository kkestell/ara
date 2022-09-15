using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record If(Node Node, Expression Predicate, Block Then) : Statement(Node);