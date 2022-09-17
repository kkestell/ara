using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values.Instructions;

public class ReturnInstruction : Instruction
{
    readonly Value? value;

    public override IrType Type => value?.Type ?? IrType.Void;

    public ReturnInstruction(Value? value, Block block) : base(block)
    {
        this.value = value;
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (value is null)
            sb.AppendLine("ret");
        else
            sb.AppendLine($"ret {value.Type.ToIr()} {value.Resolve()}");
    }
}