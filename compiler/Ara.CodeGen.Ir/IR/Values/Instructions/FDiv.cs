#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class FDiv : Instruction
{
    private readonly Value _left;
    private readonly Value _right;

    public override IrType Type => _left.Type;

    public FDiv(Function function, Value left, Value right, string? name = null) : base(function, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(FloatType))
            throw new ArgumentException();
        
        _left = left;
        _right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = fdiv {_left.Type.ToIr()} {_left.Resolve()}, {_right.Resolve()}\n");
    }
}