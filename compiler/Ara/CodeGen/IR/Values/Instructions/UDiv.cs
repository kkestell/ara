using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

/// <summary>
/// https://llvm.org/docs/LangRef.html#binary-operations
/// </summary>
public class UDiv : Instruction
{
    readonly Value left;
    readonly Value right;

    public override IrType Type => left.Type;

    public UDiv(Block block, Value left, Value right, string? name = null) : base(block, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(IntegerType))
            throw new ArgumentException();
        
        this.left = left;
        this.right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = udiv {left.Type.ToIr()} {left.Resolve()}, {right.Resolve()}\n");
    }
}