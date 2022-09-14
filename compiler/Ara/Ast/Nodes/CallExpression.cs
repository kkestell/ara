using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record CallExpression(Node Node, Identifier Name, List<Argument> Arguments) : Expression(Node);
