using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public record CallExpression(Node Node, Identifier Name, List<Argument> Arguments) : Expression(Node);
