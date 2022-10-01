using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record SourceFile
    (Node Node, NodeList<FunctionDefinition> FunctionDefinitions) : AstNode(Node)
{
    public override List<AstNode> Children { get; } = new() { FunctionDefinitions };
}
