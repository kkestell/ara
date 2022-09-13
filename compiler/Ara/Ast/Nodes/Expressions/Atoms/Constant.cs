using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public record Constant(Node Node, string Value, InferredType Type) : Atom(Node);