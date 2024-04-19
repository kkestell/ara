namespace Ara.CodeGen.IR.Types;

public record VoidType : IrType
{
    public override string ToIr() => "void";
}