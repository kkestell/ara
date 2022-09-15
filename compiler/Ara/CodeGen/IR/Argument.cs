using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class Argument
{
    public Argument(IrType type, Value value)
    {
        Type = type;
        Value = value;
    }
    
    public IrType Type { get; }
    
    public Value Value { get; }

    public string ToIr() => $"{Type.ToIr()} {Value.Resolve()}";
}