namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public enum FcmpCondition
{
    NoComparisonFalse,
    OrderedAndEqual,
    OrderedAndGreaterThan,
    OrderedAndGreaterThanOrEqual,
    OrderedAndLessThan,
    OrderedAndLessThanOrEqual,
    OrderedAndNotEqual,
    OrderedNoNaN,
    UnorderedOrEqual,
    UnorderedOrGreaterThan,
    UnorderedOrGreaterThanOrEqual,
    UnorderedOrLessThan,
    UnorderedOrLessThanOrEqual,
    UnorderedOrNotEqual,
    UnorderedEitherNaN,
    NoComparisonTrue
}