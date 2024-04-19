namespace Ara.CodeGen.IR.Types;

public record IntegerType(int Bits) : IrType
{
    public override string ToIr() => $"i{Bits}";
}