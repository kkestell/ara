#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Mul : Instruction
{
    private readonly Value _left;
    private readonly Value _right;

    public override IrType Type => _left.Type;

    public Mul(Function function, Value left, Value right, string? name = null) : base(function, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException($"Cannot add values of different types: {left.Type} and {right.Type}", nameof(right));

        if (left.Type.GetType() != typeof(IntegerType))
            throw new ArgumentException($"Cannot add values of type {left.Type}", nameof(left));
        
        _left = left;
        _right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = mul {_left.Type.ToIr()} {_left.Resolve()}, {_right.Resolve()}\n");
    }
}