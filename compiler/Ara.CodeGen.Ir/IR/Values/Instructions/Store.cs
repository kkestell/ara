#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Store : Value
{
    private readonly Value _value;
    private readonly Value _pointer;

    public override IrType Type => IrType.Void;

    public Store(Value value, Value pointer)
    {
        _value = value;
        _pointer = pointer;
    }

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.Append($"store {_value.Type.ToIr()} {_value.Resolve()}, ptr {_pointer.Resolve()}\n");
    }
}