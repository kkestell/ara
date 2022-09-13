using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Identifier(Node Node, string Value) : AstNode(Node);