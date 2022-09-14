using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Constant(Node Node, string Value, InferredType Type) : Atom(Node);