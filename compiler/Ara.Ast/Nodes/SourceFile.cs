#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes;

public record SourceFile(IParseNode Node, NodeList<FunctionDefinition> FunctionDefinitions, NodeList<ExternalFunctionDeclaration>? ExternalFunctionDeclarations = null) : AstNode(Node)
{
    public override IEnumerable<AstNode> Children => ExternalFunctionDeclarations is not null ? new List<AstNode> { ExternalFunctionDeclarations , FunctionDefinitions } : new List<AstNode> { FunctionDefinitions };
}
