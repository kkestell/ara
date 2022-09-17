using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values;

public class FloatValue : ConstantValue
{
    readonly float value;
    
    public override IrType Type => IrType.Float;

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

    public override string ToIr()
    {
        return Resolve();
    }
    
    public override void Emit(StringBuilder sb)
    {
    }
}