#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values;

public class BooleanValue : ConstantValue
{
    private readonly int _value;
    
    public override IrType Type => IrType.Bool;

    public BooleanValue(bool value)
    {
        _value = value ? 1 : 0;
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