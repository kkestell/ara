#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values;

public class FloatValue : ConstantValue
{
    private readonly float _value;
    
    public override IrType Type => IrType.Float;

    public FloatValue(float value)
    {
        _value = value;
    }

    public override string Resolve()
    {
        var bytes = BitConverter.GetBytes((double)_value);
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