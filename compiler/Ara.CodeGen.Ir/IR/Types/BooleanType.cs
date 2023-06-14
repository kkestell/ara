namespace Ara.CodeGen.Ir.IR.Types;

public record BooleanType : IrType
{
    public override string ToIr() => "i1";
}