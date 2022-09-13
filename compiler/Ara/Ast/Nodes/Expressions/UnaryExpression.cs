using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public record UnaryExpression(Node Node, Expression Right, UnaryOperator Op) : Expression(Node);
