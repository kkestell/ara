using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReferenceException : SemanticException
{
    public ReferenceException(AstNode node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(AstNode node)
    {
        var name = node switch
        {
            VariableReference r => r.Name.Value,
            Call c => c.Name.Value,
            _ => throw new Exception($"Unsupported node type {node.GetType()}")
        };
        
        return $"The name {name} is not defined.";
    }
}