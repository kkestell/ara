using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Statements;

namespace Ara.Ast.Errors;

public class ReturnTypeException : SemanticException
{
    public ReturnTypeException(Return node) : base(node.Expression, BuildMessage(node))
    {
    }

    private static string BuildMessage(Return node)
    {
        var func = node.NearestAncestor<FunctionDefinition>();
        return $"Invalid return type {node.Expression.Type} where {func.Type} was expected.";
    }
}