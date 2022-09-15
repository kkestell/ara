using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Return(Node Node, Expression Expression) : Statement(Node);
