using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values.Instructions;

public class FSub : Instruction
{
    readonly Value left;
    readonly Value right;

    public override IrType Type => left.Type;

    public FSub(Block block, Value left, Value right, string? name = null) : base(block, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(FloatType))
            throw new ArgumentException();
        
        this.left = left;
        this.right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = fsub {left.Type.ToIr()} {left.Resolve()}, {right.Resolve()}\n");
    }
}