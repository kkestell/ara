namespace Ara.CodeGen.Ir.IR.Types;

public record ArrayType(IrType Type, int Size) : IrType
{
    public override string ToIr()
    {
        return $"{{[{Size} x {Type.ToIr()}]}}";
    }
}