namespace Ara.CodeGen.IR.Types;

public record BooleanType : IrType
{
    public override string ToIr() => $"i1";
}