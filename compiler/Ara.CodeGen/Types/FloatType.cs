namespace Ara.CodeGen.Types;

public record FloatType : IrType
{
    public override string ToIr() => "float";
}