using Ara.Ast.Nodes.Statements;

namespace Ara.Ast.Errors;

public class VariableTypeException : SemanticException
{
    public VariableTypeException(VariableDeclaration node) : base(node.Expression!, BuildMessage(node))
    {
    }

    private static string BuildMessage(VariableDeclaration node)
    {
        return $"Invalid type {node.Expression!.Type} where {node.Type} was expected.";
    }
}