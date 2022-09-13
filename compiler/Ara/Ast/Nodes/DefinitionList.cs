using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class DefinitionList : AstNode
{
    public DefinitionList(Node node, IEnumerable<Definition> definitions) : base(node)
    {
        Definitions = definitions;
    }
    
    public IEnumerable<Definition> Definitions { get; }
}