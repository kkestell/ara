using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

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