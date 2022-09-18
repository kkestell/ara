using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Store : Instruction
{
    readonly Value value;
    readonly Value pointer;

    public override IrType Type => IrType.Void;

    public Store(Block block, Value value, Value pointer, string? name = null) : base(block, name)
    {
        this.value = value;
        this.pointer = pointer;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"store {value.Type.ToIr()} {value.Resolve()}, ptr {pointer.Resolve()}\n");
    }
}