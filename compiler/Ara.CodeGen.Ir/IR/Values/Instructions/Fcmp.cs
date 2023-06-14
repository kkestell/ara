#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Fcmp : Instruction
{
    private readonly FcmpCondition _condition;
    private readonly Value _left;
    private readonly Value _right;

    public Fcmp(Function function, FcmpCondition condition, Value left, Value right, string? name = null) : base(function, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(FloatType))
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
            FcmpCondition.NoComparisonFalse             => "false",
            FcmpCondition.OrderedAndEqual               => "oeq",
            FcmpCondition.OrderedAndGreaterThan         => "ogt",
            FcmpCondition.OrderedAndGreaterThanOrEqual  => "oge",
            FcmpCondition.OrderedAndLessThan            => "olt",
            FcmpCondition.OrderedAndLessThanOrEqual     => "ole",
            FcmpCondition.OrderedAndNotEqual            => "one",
            FcmpCondition.OrderedNoNaN                  => "ord",
            FcmpCondition.UnorderedOrEqual              => "ueq",
            FcmpCondition.UnorderedOrGreaterThan        => "ugt",
            FcmpCondition.UnorderedOrGreaterThanOrEqual => "uge",
            FcmpCondition.UnorderedOrLessThan           => "ult",
            FcmpCondition.UnorderedOrLessThanOrEqual    => "ule",
            FcmpCondition.UnorderedOrNotEqual           => "une",
            FcmpCondition.UnorderedEitherNaN            => "uno",
            FcmpCondition.NoComparisonTrue              => "true",
            _ => throw new NotImplementedException()
        };

        sb.Append($"{Resolve()} = fcmp {cond} {_left.Type.ToIr()} {_left.Resolve()}, {_right.Resolve()}\n");
    }
}