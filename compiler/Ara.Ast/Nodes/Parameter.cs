using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, Identifier Name, Identifier Type) : AstNode(Node);
