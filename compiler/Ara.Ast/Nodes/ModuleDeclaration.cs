using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ModuleDeclaration(Node Node, Identifier Name) : AstNode(Node);
