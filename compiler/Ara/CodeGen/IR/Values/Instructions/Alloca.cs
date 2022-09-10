using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

/// <summary>
/// https://llvm.org/docs/LangRef.html#alloca-instruction
/// </summary>
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