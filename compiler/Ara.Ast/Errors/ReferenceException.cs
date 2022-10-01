using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;

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
            VariableReference r => r.Name,
            Call c => c.Name,
            _ => throw new Exception($"Unsupported node type {node.GetType()}")
        };
        
        return $"The name {name} is not defined.";
    }
}