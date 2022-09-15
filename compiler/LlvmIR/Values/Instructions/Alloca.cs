using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values.Instructions;

public class Alloca : Instruction
{
    readonly IrType type;
    readonly int size;

    public override IrType Type => new PointerType(type);

    public Alloca(Block block, IrType type, int size, string? name = null) : base(block, name)
    {
        this.size = size;
        this.type = type;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {type.ToIr()}, align 4\n");
    }
}