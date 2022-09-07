using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record ModuleDeclaration(Node Node, Identifier Name) : AstNode(Node);