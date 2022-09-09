using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Memory;

/// <summary>
/// https://llvm.org/docs/LangRef.html#alloca-instruction
/// </summary>
public class AllocaInstruction : Instruction
{
    readonly IrType type;
    readonly int size;

    public override IrType Type => new PointerType(type);

    public AllocaInstruction(Block block, IrType type, int size, string? name = null) : base(block, name)
    {
        this.size = size;
        this.type = type;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {type.ToIr()}, align 4\n");
    }
}