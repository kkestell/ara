using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record VariableReference(Node Node, Identifier Name) : Atom(Node);
