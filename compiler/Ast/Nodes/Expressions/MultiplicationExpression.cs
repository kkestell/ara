using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public record MultiplicationExpression(Node Node, Expression Left, Expression Right) : BinaryExpression(Node, Left, Right);
