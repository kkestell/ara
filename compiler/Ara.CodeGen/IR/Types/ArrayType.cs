namespace Ara.CodeGen.IR.Types;

public record ArrayType(IrType Type) : IrType
{
    public override string ToIr()
    {
        return $"{{i32, [0 x {Type.ToIr()}]}}";
    }
}