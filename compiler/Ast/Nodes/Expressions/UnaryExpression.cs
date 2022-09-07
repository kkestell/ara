using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions;

public abstract record UnaryExpression(Node Node, Expression Right) : Expression(Node);
