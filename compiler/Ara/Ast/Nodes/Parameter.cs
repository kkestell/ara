using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, Identifier Name, Type_ Type) : AstNode(Node);
