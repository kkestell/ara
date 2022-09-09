namespace Ara.CodeGen.IR.Values.Abstract;

public abstract class ConstantValue : Value
{
    public abstract string ToIr();
}