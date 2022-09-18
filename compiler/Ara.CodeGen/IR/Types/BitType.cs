namespace Ara.CodeGen.IR.Types;

public record BitType : IrType
{
    public override string ToIr() => $"i1";
}