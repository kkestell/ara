using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record SourceFile(Node Node, ModuleDeclaration ModuleDeclaration, IEnumerable<Definition> Definitions) : AstNode(Node);
