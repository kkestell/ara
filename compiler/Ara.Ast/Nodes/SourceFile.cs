using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record SourceFile
    (Node Node, NodeList<FunctionDefinition> Definitions) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Definitions };
}
