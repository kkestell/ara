namespace Ara.CodeGen.IR.Values.Instructions;

public abstract class Instruction : NamedValue
{
    protected Instruction(Block block, string? name = null) : base(block, name)
    {
    }
}