using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record Integer(Node Node, string Value) : Atom(Node);