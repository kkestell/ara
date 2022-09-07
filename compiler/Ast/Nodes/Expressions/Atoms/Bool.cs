using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record Bool(Node Node, string Value) : Atom(Node);