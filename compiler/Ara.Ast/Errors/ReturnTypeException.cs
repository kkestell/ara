using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReturnTypeException : SemanticException
{
    public ReturnTypeException(Return node) : base(node.Expression, BuildMessage(node))
    {
    }

    static string BuildMessage(Return node)
    {
        var func = node.NearestAncestor<FunctionDefinition>();
        return $"Invalid return type {node.Expression.Type} where {func.Type} was expected.";
    }
}