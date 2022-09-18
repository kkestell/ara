using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReferenceException : SemanticException
{
    ReferenceException(AstNode node, string message) : base(node.Node, message)
    {
    }

    public static ReferenceException Create(AstNode node)
    {
        var name = node switch
        {
            VariableReference r => r.Name.Value,
            Call c => c.Name.Value,
            _ => throw new Exception($"Unsupported node type {node.GetType()}")
        };
        
        var message = $"The name {name} is not defined.";
        return new ReferenceException(node, message);
    }
}