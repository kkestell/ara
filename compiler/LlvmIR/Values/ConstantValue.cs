namespace LlvmIR.Values;

public abstract class ConstantValue : Value
{
    public abstract string ToIr();
}