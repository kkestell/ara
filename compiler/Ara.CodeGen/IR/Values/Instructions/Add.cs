#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class Add : Instruction
{
    private readonly Value _left;
    private readonly Value _right;

    public override IrType Type => _left.Type;

    public Add(Function function, Value left, Value right, string? name = null) : base(function, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(IntegerType))
            throw new ArgumentException();
        
        _left = left;
        _right = right;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = add {_left.Type.ToIr()} {_left.Resolve()}, {_right.Resolve()}\n");
    }
}