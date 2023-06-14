#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Alloca : Instruction
{
    private readonly IrType _type;
    private readonly int _size;

    public Alloca(Function function, IrType type, int size, string? name = null) : base(function, name)
    {
        _type = type;
        _size = size;
    }

    public override IrType Type => _size == 1 ? new PointerType(_type) : new ArrayType(_type, _size);

    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {_type.ToIr()}, i32 {_size}, align 4\n");
    }
}