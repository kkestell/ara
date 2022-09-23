using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Call(Node Node, string Name, List<Argument> Arguments) : Expression(Node);
