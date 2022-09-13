using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class ParameterList : AstNode
{
    public ParameterList(Node node, IEnumerable<Parameter> parameters) : base(node)
    {
        Parameters = parameters;
    }
    
    public IEnumerable<Parameter> Parameters { get; }
}
