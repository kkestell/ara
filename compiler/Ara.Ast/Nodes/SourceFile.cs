using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record SourceFile
    (Node Node, ModuleDeclaration ModuleDeclaration, NodeList<Definition> Definitions) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { ModuleDeclaration, Definitions };
}
