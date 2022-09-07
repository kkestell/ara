using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public record LogicalNegationExpression(Node Node, Expression Right) : UnaryExpression(Node, Right);
