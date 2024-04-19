#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class Alloca : Instruction
{
    private readonly IrType _type;
    private readonly List<int>? _sizes;

    public Alloca(Function function, IrType type, List<int>? sizes, string? name = null) : base(function, name)
    {
        _type = type;
        _sizes = sizes;
    }

    public override IrType Type => _sizes is null ? new PointerType(_type) : new ArrayType(_type, _sizes);

    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = alloca {Type.ToIr()}, align 4\n");
    }
}