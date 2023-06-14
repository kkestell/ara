#region

using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Ir.IR;

public class Argument
{
    private readonly IrType _type;
    private readonly Value _value;

    public Argument(IrType type, Value value)
    {
        _type = type;
        _value = value;
    }
    
    public string ToIr() => $"{_type.ToIr()} {_value.Resolve()}";
}