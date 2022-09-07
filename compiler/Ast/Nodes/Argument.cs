using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record Argument(Node Node, Identifier Name, Expression Expression) : AstNode(Node);
