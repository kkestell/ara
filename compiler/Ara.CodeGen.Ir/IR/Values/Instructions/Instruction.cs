namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public abstract class Instruction : NamedValue
{
    protected Instruction(Function function, string? name = null) : base(function, name)
    {
    }
}