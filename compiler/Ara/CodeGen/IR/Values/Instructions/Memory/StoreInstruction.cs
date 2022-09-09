using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Memory;

/// <summary>
/// https://llvm.org/docs/LangRef.html#store-instruction
/// </summary>
public class StoreInstruction : Instruction
{
    readonly Value value;
    readonly Value pointer;

    public override IrType Type => new VoidType();

    public StoreInstruction(Block block, Value value, Value pointer, string? name = null) : base(block, name)
    {
        this.value = value;
        this.pointer = pointer;
    }
    
    public override void Emit(StringBuilder sb)
    {
        value.Emit(sb);
        sb.Append($"store {value.Type.ToIr()} {value.Resolve()}, ptr {pointer.Resolve()}\n");
    }
}