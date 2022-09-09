using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public record FunctionCallExpression(Node Node, Identifier Name, List<Argument> Arguments) : Expression(Node);
