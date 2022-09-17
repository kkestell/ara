using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class BinaryExpressionTypeException : CompilerException
{
    readonly BinaryExpression astNode;

    public BinaryExpressionTypeException(BinaryExpression astNode) : base(astNode.Node)
    {
        this.astNode = astNode;
        Description = "Binary expression left hand side {astNode.Left.InferredType!.Value} doesn't match right hand side {astNode.Right.InferredType!.Value}.";
    }
}