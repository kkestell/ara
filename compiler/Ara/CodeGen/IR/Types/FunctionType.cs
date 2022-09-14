using System.Text;

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
}