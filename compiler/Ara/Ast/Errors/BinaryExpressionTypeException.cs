using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class BinaryExpressionTypeException : CompilerException
{
    readonly BinaryExpression astNode;

    public BinaryExpressionTypeException(BinaryExpression astNode) : base(astNode.Node)
    {
        this.astNode = astNode;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("BinaryExpressionTypeException");
        sb.AppendLine(Location.ToString());
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented($"Binary expression left hand side {astNode.Left.InferredType!.Value} doesn't match right hand side {astNode.Right.InferredType!.Value}"));
        
        return sb.ToString();
    }
}