using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class BinaryExpressionTypeException : SemanticException
{
    BinaryExpressionTypeException(BinaryExpression node, string message) : base(node.Node, message)
    {
    }

    public static BinaryExpressionTypeException Create(BinaryExpression node)
    {
        var message = $"Binary expression left hand side {node.Left.InferredType!.Value} doesn't match right hand side {node.Right.InferredType!.Value}.";
        return new BinaryExpressionTypeException(node, message);
    }
}