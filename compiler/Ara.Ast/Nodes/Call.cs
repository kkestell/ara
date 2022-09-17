using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Call(Node Node, Identifier Name, List<Argument> Arguments) : Expression(Node);
