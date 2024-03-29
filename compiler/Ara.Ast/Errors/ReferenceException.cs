#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;

#endregion

namespace Ara.Ast.Errors;

public class ReferenceException : SemanticException
{
    public ReferenceException(AstNode node) : base(node, BuildMessage(node))
    {
    }

    private static string BuildMessage(AstNode node)
    {
        var name = node switch
        {
            VariableReference r => r.Name,
            CallExpression c => c.Name,
            _ => throw new Exception($"Unsupported node type {node.GetType()}")
        };
        
        return $"The name {name} is not defined.";
    }
}