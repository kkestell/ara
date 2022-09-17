namespace Ara.CodeGen.Types;

public record VoidType : IrType
{
    public override string ToIr() => "void";
}