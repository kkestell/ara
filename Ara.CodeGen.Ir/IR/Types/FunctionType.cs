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

    public static FunctionType FromDefinition(Ara.Parsing.Nodes.FunctionDefinition functionDefinition)
    {
        return new FunctionType(
            IrType.FromType(functionDefinition.TypeRef),
            functionDefinition.Parameters.Select(x =>
                new Parameter(x.Identifier.Value, IrType.FromType(x.TypeRef))).ToList());
    }
    
    // public static FunctionType FromExternalDeclaration(Ara.Parsing.Nodes.ExternalFunctionDeclaration externalFunctionDeclaration)
    // {
    //     return new FunctionType(
    //         IrType.FromType(externalFunctionDeclaration.Type),
    //         externalFunctionDeclaration.Parameters.Nodes.Select(x =>
    //             new Parameter(x.Name, IrType.FromType(x.Type))).ToList());
    // }
}