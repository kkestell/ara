using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class BinaryExpressionTypeException : SemanticException
{
    public BinaryExpressionTypeException(BinaryExpression node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(BinaryExpression node)
    {
        return $"Binary expression left hand side {node.Left.Type} doesn't match right hand side {node.Right.Type}.";
    }
}