using Ara.Ast.Nodes;

namespace Ara.CodeGen.IR.Types;

public class FunctionType
{
    public FunctionType(IrType? returnType = null, List<Parameter>? parameters = null)
    {
        ReturnType = returnType ?? IrType.Void;
        Parameters = parameters ?? new List<Parameter>();
    }

    public IrType ReturnType { get; }
    
    public IReadOnlyList<Parameter> Parameters { get; }

    public static FunctionType FromDefinition(FunctionDefinition functionDefinition)
    {
        return new FunctionType(
            IrType.FromType(functionDefinition.Type),
            functionDefinition.Parameters.Nodes.Select(x =>
                new Parameter(x.Name, IrType.FromType(x.Type))).ToList());
    }
}