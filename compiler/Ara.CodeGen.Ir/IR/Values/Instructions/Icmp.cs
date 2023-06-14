#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Icmp : Instruction
{
    private readonly IcmpCondition _condition;
    private readonly Value _left;
    private readonly Value _right;

    public Icmp(Function function, IcmpCondition condition, Value left, Value right, string? name = null) : base(function, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type is not IntegerType and not BooleanType and not PointerType)
            throw new ArgumentException();

        _condition = condition;
        _left = left;
        _right = right;
    }

    public override IrType Type => IrType.Bool;

    public override void Emit(StringBuilder sb)
    {
        var cond = _condition switch
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

        sb.Append($"{Resolve()} = icmp {cond} {_left.Type.ToIr()} {_left.Resolve()}, {_right.Resolve()}\n");
    }
}