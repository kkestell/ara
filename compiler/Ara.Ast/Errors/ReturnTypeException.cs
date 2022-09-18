using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReturnTypeException : SemanticException
{
    public ReturnTypeException(Return node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(Return node)
    {
        var func = node.NearestAncestor<FunctionDefinition>()!;
        return $"Invalid return type {node.Expression.InferredType!.Value} where {func.InferredType!.Value} was expected.";
    }
}