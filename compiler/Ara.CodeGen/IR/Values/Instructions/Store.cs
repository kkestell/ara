using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Store : Value
{
    readonly Value value;
    readonly Value pointer;

    public override IrType Type => IrType.Void;

    public Store(Value value, Value pointer)
    {
        this.value = value;
        this.pointer = pointer;
    }

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.Append($"store {value.Type.ToIr()} {value.Resolve()}, ptr {pointer.Resolve()}\n");
    }
}