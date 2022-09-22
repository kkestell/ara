namespace Ara.CodeGen.IR.Types;

public record ArrayType(IrType Type, int Size) : IrType
{
    public override string ToIr()
    {
        return $"{{[{Size} x {Type.ToIr()}]}}";
    }
}