using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ReturnStatement(Node Node, Expression Expression) : Statement(Node);
