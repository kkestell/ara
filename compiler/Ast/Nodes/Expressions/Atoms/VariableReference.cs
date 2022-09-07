using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record VariableReference(Node Node, Identifier Name) : Atom(Node);
