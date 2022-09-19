using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public class BooleanValue : ConstantValue
{
    readonly int value;
    
    public override IrType Type => IrType.Bool;

    public BooleanValue(bool value)
    {
        this.value = value ? 1 : 0;
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