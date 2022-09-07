using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record TypeNode(Node Node, string Value) : AstNode(Node);
