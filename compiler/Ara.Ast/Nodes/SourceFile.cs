#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes;

public record SourceFile(IParseNode Node, NodeList<AstNode> Definitions, NodeList<ExternalFunctionDeclaration>? ExternalFunctionDeclarations = null) : AstNode(Node)
{
    public override IEnumerable<AstNode> Children => ExternalFunctionDeclarations is not null ? new List<AstNode> { ExternalFunctionDeclarations , Definitions } : new List<AstNode> { Definitions };
    
    public IEnumerable<FunctionDefinition> FunctionDefinitions => Definitions.Nodes.OfType<FunctionDefinition>();
    
    public IEnumerable<StructDefinition> StructDefinitions => Definitions.Nodes.OfType<StructDefinition>();
}
