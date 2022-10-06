using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record SourceFile(IParseNode Node, NodeList<FunctionDefinition> FunctionDefinitions) : AstNode(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { FunctionDefinitions };
}
