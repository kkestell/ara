using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Operations.Abstract;

/// <summary>
/// https://llvm.org/docs/LangRef.html#binary-operations
/// </summary>
public abstract class BinaryOperationInstruction : Instruction
{
    protected readonly Value Left;
    protected readonly Value Right;

    public override IrType Type => Left.Type;

    protected BinaryOperationInstruction(Block block, Value left, Value right, string? name = null) : base(block, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException($"Types don't match {left.Type.ToIr()} vs. {right.Type.ToIr()}");

        if (left.Type.GetType() != typeof(IntegerType))
            throw new ArgumentException($"Arguments must be {typeof(IntegerType)}, not {left.Type.GetType()}");
        
        Left = left;
        Right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        Left.Emit(sb);
        Right.Emit(sb);
    }
}