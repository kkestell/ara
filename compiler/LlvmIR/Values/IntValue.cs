using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values;

public class IntValue : ConstantValue
{
    readonly int value;
    
    public override IrType Type => IrType.Int32;

    public IntValue(int value)
    {
        this.value = value;
    }

    public override string Resolve()
    {
        return value.ToString();
    }

    public override void Emit(StringBuilder sb)
    {
    }

    public override string ToIr()
    {
        return value.ToString();
    }
}