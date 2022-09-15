using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values.Instructions;

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