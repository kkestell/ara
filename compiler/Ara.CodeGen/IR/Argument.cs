using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class Argument
{
    readonly IrType type;
    readonly Value value;

    public Argument(IrType type, Value value)
    {
        this.type = type;
        this.value = value;
    }
    
    public string ToIr() => $"{type.ToIr()} {value.Resolve()}";
}