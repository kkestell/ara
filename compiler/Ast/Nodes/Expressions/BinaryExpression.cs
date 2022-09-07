using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public abstract record BinaryExpression(Node Node, Expression Left, Expression Right) : Expression(Node);
