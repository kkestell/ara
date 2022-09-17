namespace Ara.CodeGen.Types;

public record BitType : IrType
{
    public override string ToIr() => $"i1";
}