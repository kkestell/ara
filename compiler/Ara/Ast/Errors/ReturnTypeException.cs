using System.Text;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Statements;

namespace Ara.Ast.Errors;

public class ReturnTypeException : CompilerException
{
    readonly ReturnStatement returnStatement;
    
    public ReturnTypeException(ReturnStatement returnStatement) : base(returnStatement.Node)
    {
        this.returnStatement = returnStatement;
    }

    public override string ToString()
    {
        var func = returnStatement.NearestAncestor<FunctionDefinition>();

        var sb = new StringBuilder();
        
        sb.AppendLine($"fn {func.Name.Node.Span.ToString()} ({string.Join(", ", func.Parameters.Select(p => p.Node.Span.ToString()))}) -> {func.ReturnType.Node.Span.ToString()} {{");
        sb.AppendLine($"  return {returnStatement.Expression.Node.Span.ToString()}");
        sb.Append($"         ⮤ Invalid return type `{returnStatement.Expression.InferredType?.Value}` where `{func.InferredType.Value}` was expected");

        return sb.ToString();
    }
}