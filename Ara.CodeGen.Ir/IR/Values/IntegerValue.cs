#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values;

public class IntegerValue : ConstantValue
{
    private readonly int _value;
    
    public override IrType Type => IrType.Integer;

    public IntegerValue(int value)
    {
        _value = value;
    }

    public override string Resolve()
    {
        return _value.ToString();
    }

    public override void Emit(StringBuilder sb)
    {
    }

    public override string ToIr()
    {
        return _value.ToString();
    }
}