namespace Ara.CodeGen.IR.Values;

public abstract class ConstantValue : Value
{
    public abstract string ToIr();
}