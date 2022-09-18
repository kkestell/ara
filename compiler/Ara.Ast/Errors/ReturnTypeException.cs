using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReturnTypeException : SemanticException
{
    ReturnTypeException(AstNode node, string message) : base(node.Node, message)
    {
    }

    public static ReturnTypeException Create(Return node)
    {
        var func = node.NearestAncestor<FunctionDefinition>()!;
        var message = $"Invalid return type {node.Expression.InferredType!.Value} where {func.InferredType!.Value} was expected.";
        
        return new ReturnTypeException(node, message);
    }
}