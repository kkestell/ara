using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReturnTypeException : CompilerException
{
    readonly Return astNode;
    
    public ReturnTypeException(Return astNode) : base(astNode.Node)
    {
        this.astNode = astNode;
    }

    public override string ToString()
    {
        var func = astNode.NearestAncestor<FunctionDefinition>()!;

        var sb = new StringBuilder();

        sb.AppendLine("ReturnTypeException");
        sb.AppendLine(Location.ToString());
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented($"Invalid return type {astNode.Expression.InferredType!.Value} where {func.InferredType!.Value} was expected"));
        
        return sb.ToString();
    }
}