using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Alloca : Instruction
{
    readonly IrType type;
    readonly int size;

    public Alloca(Block block, IrType type, int size, string? name = null) : base(block, name)
    {
        this.type = type;
        this.size = size;
    }

    public override IrType Type => size == 1 ? new PointerType(type) : new ArrayType(type, size);

    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {type.ToIr()}, i32 {size}, align 4\n");
    }
}