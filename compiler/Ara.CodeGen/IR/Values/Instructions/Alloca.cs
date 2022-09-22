using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Alloca : Instruction
{
    readonly IrType type;

    public override IrType Type => new PointerType(type);

    public Alloca(Block block, IrType type, string? name = null) : base(block, name)
    {
        this.type = type;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {type.ToIr()}, align 4\n");
    }
}