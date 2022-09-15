namespace LlvmIR.Types;

public record IntType(int Bits) : IrType
{
    public override string ToIr() => $"i{Bits}";
}