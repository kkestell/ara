using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record SourceFile(IParseNode Node, NodeList<FunctionDefinition> FunctionDefinitions) : AstNode(Node)
{
    readonly AstNode[] children = { FunctionDefinitions };

    public override IEnumerable<AstNode> Children => children;
}
