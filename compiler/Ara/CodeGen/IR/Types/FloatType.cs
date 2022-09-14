namespace Ara.CodeGen.IR.Types;

public record FloatType : IrType
{
    public override string ToIr() => "float";
}