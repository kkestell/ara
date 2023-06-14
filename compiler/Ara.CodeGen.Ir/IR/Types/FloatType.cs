namespace Ara.CodeGen.Ir.IR.Types;

public record FloatType : IrType
{
    public override string ToIr() => "float";
}