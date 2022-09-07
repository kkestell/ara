using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, Identifier Name, TypeNode Type) : AstNode(Node);
