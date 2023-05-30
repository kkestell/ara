#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class ReturnInstruction : Instruction
{
    private readonly Value? _value;

    public override IrType Type => _value?.Type ?? IrType.Void;

    public ReturnInstruction(Value? value, Function function) : base(function)
    {
        _value = value;
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (_value is null)
            sb.AppendLine("ret void");
        else
            sb.AppendLine($"ret {_value.Type.ToIr()} {_value.Resolve()}");
    }
}