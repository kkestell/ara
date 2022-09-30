using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Icmp : Instruction
{
    readonly IcmpCondition condition;
    readonly Value left;
    readonly Value right;

    public Icmp(Block block, IcmpCondition condition, Value left, Value right, string? name = null) : base(block, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type is not IntegerType and not BooleanType and not PointerType)
            throw new ArgumentException();

        this.condition = condition;
        this.left = left;
        this.right = right;
    }

    public override IrType Type => IrType.Bool;

    public override void Emit(StringBuilder sb)
    {
        var cond = condition switch
        {
            IcmpCondition.Equal                  => "eq",
            IcmpCondition.NotEqual               => "ne",
            IcmpCondition.UnsignedGreaterThan    => "ugt",
            IcmpCondition.UnsignedGreaterOrEqual => "uge",
            IcmpCondition.UnsignedLessThan       => "ult",
            IcmpCondition.UnsignedLessOrEqual    => "ule",
            IcmpCondition.SignedGreaterThan      => "sgt",
            IcmpCondition.SignedGreaterOrEqual   => "sge",
            IcmpCondition.SignedLessThan         => "slt",
            IcmpCondition.SignedLessOrEqual      => "sle",
            _ => throw new NotImplementedException()
        };

        sb.Append($"{Resolve()} = icmp {cond} {left.Type.ToIr()} {left.Resolve()}, {right.Resolve()}\n");
    }
}