namespace Ara.CodeGen.Types;

public record StringType : IrType
{
    public override string ToIr() => "%string";
}