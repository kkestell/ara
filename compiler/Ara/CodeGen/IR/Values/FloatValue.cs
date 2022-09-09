using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Abstract;

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
        var bytes = BitConverter.GetBytes(value);
        var i = BitConverter.ToInt32(bytes, 0);
        return "0x" + i.ToString("X8");
    }

    public override void Emit(StringBuilder sb)
    {
    }

    public override string ToIr()
    {
        return Resolve();
    }
}