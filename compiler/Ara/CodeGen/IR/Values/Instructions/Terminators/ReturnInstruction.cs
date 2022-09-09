using System.Text;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Terminators;

/// <summary>
/// https://llvm.org/docs/LangRef.html#ret-instruction
/// </summary>
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
        value.Emit(sb);
        sb.Append($"ret {value.Type.ToIr()} {value.Resolve()}\n");
    }
}