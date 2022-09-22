namespace Ara.CodeGen.IR.Types;

public record StringType : IrType
{
    public override string ToIr() => "%string";
}