using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public class IntegerValue : ConstantValue
{
    readonly int value;
    
    public override IrType Type => new IntegerType(32);

    public IntegerValue(int value)
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