namespace Ara.CodeGen.Ir.IR.Values;

public abstract class ConstantValue : Value
{
    public abstract string ToIr();
}