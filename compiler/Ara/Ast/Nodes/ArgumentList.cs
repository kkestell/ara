using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ArgumentList(Node Node, IEnumerable<Argument> Values) : AstNode(Node);
