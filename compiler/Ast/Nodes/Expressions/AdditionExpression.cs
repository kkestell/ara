using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public record AdditionExpression(Node Node, Expression Left, Expression Right) : BinaryExpression(Node, Left, Right);
