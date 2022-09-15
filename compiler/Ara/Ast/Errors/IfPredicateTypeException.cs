using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class IfPredicateTypeException : CompilerException
{
    readonly If ifStatement;

    public IfPredicateTypeException(If ifStatement) : base(ifStatement.Node)
    {
        this.ifStatement = ifStatement;
    }

    public override string ToString()
    {
        var func = ifStatement.NearestAncestor<FunctionDefinition>();

        var sb = new StringBuilder();
        
        sb.AppendLine($"fn {func.Name.Node.Span.ToString()} ({string.Join(", ", func.Parameters.Select(p => p.Node.Span.ToString()))}) -> {func.ReturnType.Node.Span.ToString()} {{");
        sb.AppendLine($"  if {ifStatement.Predicate.Node.Span.ToString()} {{");
        sb.Append($"     ↑ Invalid predicate type `{ifStatement.Predicate.InferredType?.Value}` where `bool` was expected");

        return sb.ToString();
    }
}