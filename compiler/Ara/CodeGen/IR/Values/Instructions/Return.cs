using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class ReturnInstruction : Instruction
{
    readonly Value value;

    public override IrType Type => value.Type;

    public ReturnInstruction(Value value, Block block) : base(block)
    {
        this.value = value;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"ret {value.Type.ToIr()} {value.Resolve()}\n");
    }
}