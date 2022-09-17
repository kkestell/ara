using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record BinaryExpression(Node Node, Expression Left, Expression Right, BinaryOperator Op) : Expression(Node);