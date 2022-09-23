using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record VariableReference(Node Node, string Name) : Atom(Node);
