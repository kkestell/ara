using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record Float(Node Node, string Value) : Atom(Node);
