using System.Text;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Statements;

namespace Ara.Ast.Errors;

public class BinaryExpressionTypeException : CompilerException
{
    readonly BinaryExpression binaryExpression;

    public BinaryExpressionTypeException(BinaryExpression binaryExpression) : base(binaryExpression.Node)
    {
        this.binaryExpression = binaryExpression;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        var statement = binaryExpression.NearestAncestor<Statement>();

        sb.AppendLine("---");
        sb.AppendLine(statement.Node.Span.ToString());
        sb.AppendLine("---");
        sb.AppendLine($"Binary expression left hand side `{binaryExpression.Left.Node.Span.ToString()}` ({binaryExpression.Left.InferredType.Value}) doesn't match right hand side `{binaryExpression.Right.Node.Span.ToString()}` ({binaryExpression.Right.InferredType.Value})");

        //var func = ifStatement.NearestAncestor<FunctionDefinition>();

        
        //sb.AppendLine($"fn {func.Name.Node.Span.ToString()} ({string.Join(", ", func.Parameters.Select(p => p.Node.Span.ToString()))}) -> {func.ReturnType.Node.Span.ToString()} {{");
        //sb.AppendLine($"  if {ifStatement.Predicate.Node.Span.ToString()} {{");
        //sb.Append($"     ⮤ Invalid predicate type `{ifStatement.Predicate.InferredType?.Value}` where `bool` was expected");

        return sb.ToString();
    }
}