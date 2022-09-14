namespace Ara.CodeGen.IR.Types;

/// <summary>
/// https://llvm.org/docs/LangRef.html#integer-type
/// </summary>
public class IntType : IrType
{
    public IntType(int bits)
    {
        Bits = bits;
    }

    public int Bits { get; }

    public override string ToIr() => $"i{Bits}";

    public override bool Equals(object? obj)
    {
        if (obj is not IntType other)
            return false;

        return other.Bits == Bits;
    }
}