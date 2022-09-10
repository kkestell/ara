namespace Ara.CodeGen.IR.Types;

/// <summary>
/// https://llvm.org/docs/LangRef.html#integer-type
/// </summary>
public class IntegerType : IrType
{
    public IntegerType(int bits)
    {
        Bits = bits;
    }

    public int Bits { get; }

    public override string ToIr() => $"i{Bits}";

    public override bool Equals(object? obj)
    {
        if (obj is not IntegerType other)
            return false;

        return other.Bits == Bits;
    }
}