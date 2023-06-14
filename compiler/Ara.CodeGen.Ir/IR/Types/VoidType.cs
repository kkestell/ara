namespace Ara.CodeGen.Ir.IR.Types;

public record VoidType : IrType
{
    public override string ToIr() => "void";
}