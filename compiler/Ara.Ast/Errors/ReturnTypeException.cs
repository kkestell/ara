using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReturnTypeException : CompilerException
{
    public ReturnTypeException(Return astNode) : base(astNode.Node)
    {
        var func = astNode.NearestAncestor<FunctionDefinition>()!;
        Description = $"Invalid return type {astNode.Expression.InferredType!.Value} where {func.InferredType!.Value} was expected.";
    }
}