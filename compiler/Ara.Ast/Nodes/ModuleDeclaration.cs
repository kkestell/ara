using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ModuleDeclaration(Node Node, string Name) : AstNode(Node);
