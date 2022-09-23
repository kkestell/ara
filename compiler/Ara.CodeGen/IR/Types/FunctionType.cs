using Ara.Ast.Nodes;

namespace Ara.CodeGen.IR.Types;

public class FunctionType
{
    public FunctionType(IrType? returnType = null, IReadOnlyList<Parameter>? parameters = null)
    {
        ReturnType = returnType ?? IrType.Void;
        Parameters = parameters ?? Array.Empty<Parameter>();
    }

    public IrType ReturnType { get; }
    
    public IReadOnlyList<Parameter> Parameters { get; }

    public static FunctionType FromDefinition(FunctionDefinition functionDefinition)
    {
        return new FunctionType(
            IrType.FromType(functionDefinition.Type),
            functionDefinition.Parameters.Select(x =>
                new Parameter(x.Name, IrType.FromType(x.Type))).ToList());
    }
}