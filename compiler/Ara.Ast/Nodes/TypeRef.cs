using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record TypeRef(Node Node, string Value) : AstNode(Node);