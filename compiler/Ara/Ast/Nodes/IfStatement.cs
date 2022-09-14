using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record IfStatement(Node Node, Expression Predicate, Block Then) : Statement(Node);