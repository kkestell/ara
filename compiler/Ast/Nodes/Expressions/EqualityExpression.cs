using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public record EqualityExpression(Node Node, Expression Left, Expression Right) : BinaryExpression(Node, Left, Right);
