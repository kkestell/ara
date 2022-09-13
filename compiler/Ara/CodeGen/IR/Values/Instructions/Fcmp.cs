using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Fcmp : Instruction
{
    readonly FcmpCondition condition;
    readonly Value left;
    readonly Value right;

    public Fcmp(Block block, FcmpCondition condition, Value left, Value right, string? name = null) : base(block, name)
    {
        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() != typeof(FloatType))
            throw new ArgumentException();

        this.condition = condition;
        this.left = left;
        this.right = right;
    }

    public override IrType Type => new IntegerType(1);

    public override void Emit(StringBuilder sb)
    {
        var cond = condition switch
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

        sb.Append($"{Resolve()} = fcmp {cond} {left.Type.ToIr()} {left.Resolve()}, {right.Resolve()}\n");
    }
}