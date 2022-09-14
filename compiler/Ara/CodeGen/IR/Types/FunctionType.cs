using System.Text;

namespace Ara.CodeGen.IR.Types;

public class FunctionType
{
    public FunctionType(IrType? returnType = null, IReadOnlyList<Parameter>? parameters = null)
    {
        ReturnType = returnType ?? new VoidType();
        Parameters = parameters ?? Array.Empty<Parameter>();
    }

    public IrType ReturnType { get; }
    
    public IReadOnlyList<Parameter> Parameters { get; }

    public void Emit(StringBuilder sb)
    {
        sb.Append($"{ReturnType.ToIr()} ({string.Join(", ", Parameters.Select(x => x.ToIr()))})");
    }
}