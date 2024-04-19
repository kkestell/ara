#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class Select : Instruction
{
    private readonly Value _cond;
    private readonly Value _value1;
    private readonly Value _value2;

    public override IrType Type => _value1.Type;

    public Select(Function function, Value cond, Value value1, Value value2, string? name = null) : base(function, name)
    {
        if (!cond.Type.Equals(IrType.Bool))
            throw new ArgumentException($"Condition has a type of {cond.Type.ToIr()}, but an i1 was expected");
        
        if (!value1.Type.Equals(value2.Type))
            throw new ArgumentException($"Type of first value {value1.Type.ToIr()} doesn't match second value {value2.Type.ToIr()}");

        _cond = cond;
        _value1 = value1;
        _value2 = value2;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = select {_cond.Type.ToIr()} {_cond.Resolve()}, {_value1.Type.ToIr()} {_value1.Resolve()}, {_value2.Type.ToIr()} {_value2.Resolve()}\n");
    }
}