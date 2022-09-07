using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public record NegationExpression(Node Node, Expression Right) : UnaryExpression(Node, Right);
