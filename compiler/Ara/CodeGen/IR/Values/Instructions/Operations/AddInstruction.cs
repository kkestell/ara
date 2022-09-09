using System.Text;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Operations.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Operations;

/// <summary>
/// https://llvm.org/docs/LangRef.html#binary-operations
/// </summary>
public class AddInstruction : BinaryOperationInstruction
{
    public AddInstruction(Block block, Value left, Value right, string? name = null) : base(block, left, right, name)
    {
    }
    
    public override void Emit(StringBuilder sb)
    {
        base.Emit(sb);
        sb.Append($"{Resolve()} = add {Left.Type.ToIr()} {Left.Resolve()}, {Right.Resolve()}\n");
    }
}