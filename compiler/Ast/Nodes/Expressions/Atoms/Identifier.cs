using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record Identifier(Node Node, string Value) : Atom(Node);