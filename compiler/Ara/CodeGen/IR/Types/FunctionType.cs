using System.Text;
using Ara.CodeGen.IR.Types.Abstract;

namespace Ara.CodeGen.IR.Types;

public class FunctionType
{
    public IrType ReturnType { get; }
    public IEnumerable<Parameter> Parameters { get; }
    
    public FunctionType(IrType? returnType = null, IEnumerable<Parameter>? parameters = null)
    {
        ReturnType = returnType ?? new VoidType();
        Parameters = parameters ?? Array.Empty<Parameter>();
    }

    public void Emit(StringBuilder sb)
    {
        sb.Append($"{ReturnType.ToIr()} ({string.Join(", ", Parameters.Select(x => x.ToIr()))})");
    }
}