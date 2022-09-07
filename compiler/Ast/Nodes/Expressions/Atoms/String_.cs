using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record String_(Node Node, string Value) : Atom(Node);
