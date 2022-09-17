using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class IfPredicateTypeException : CompilerException
{
    readonly If astNode;

    public IfPredicateTypeException(If astNode) : base(astNode.Node)
    {
        this.astNode = astNode;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("IfPredicateTypeException");
        sb.AppendLine(Location.ToString());
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented($"Invalid predicate type {astNode.Predicate.InferredType!.Value} where bool was expected"));
        
        return sb.ToString();
    }
}