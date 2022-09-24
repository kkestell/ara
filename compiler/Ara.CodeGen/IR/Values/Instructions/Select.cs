using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Select : Instruction
{
    readonly Value cond;
    readonly Value value1;
    readonly Value value2;

    public override IrType Type => value1.Type;

    public Select(Block block, Value cond, Value value1, Value value2, string? name = null) : base(block, name)
    {
        if (!cond.Type.Equals(IrType.Bool))
            throw new ArgumentException($"Condition has a type of {cond.Type.ToIr()}, but an i1 was expected");
        
        if (!value1.Type.Equals(value2.Type))
            throw new ArgumentException($"Type of first value {value1.Type.ToIr()} doesn't match second value {value2.Type.ToIr()}");

        this.cond = cond;
        this.value1 = value1;
        this.value2 = value2;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = select {cond.Type.ToIr()} {cond.Resolve()}, {value1.Type.ToIr()} {value1.Resolve()}, {value2.Type.ToIr()} {value2.Resolve()}\n");
    }
}