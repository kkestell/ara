namespace Ara.CodeGen.Ir.IR.Types;

public record IntegerType(int Bits) : IrType
{
    public override string ToIr() => $"i{Bits}";
}