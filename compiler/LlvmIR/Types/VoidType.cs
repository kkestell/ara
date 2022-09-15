namespace LlvmIR.Types;

public record VoidType : IrType
{
    public override string ToIr() => "void";
}