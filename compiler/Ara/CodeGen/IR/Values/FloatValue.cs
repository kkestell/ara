using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public class FloatValue : ConstantValue
{
    readonly float value;
    
    public override IrType Type => new FloatType();

    public FloatValue(float value)
    {
        this.value = value;
    }

    public override string Resolve()
    {
        var bytes = BitConverter.GetBytes((double)value);
        var i = BitConverter.ToInt64(bytes, 0);
        return $"0x{i:X16}";
    }

    public override void Emit(StringBuilder sb)
    {
    }

    public override string ToIr()
    {
        return Resolve();
    }
}