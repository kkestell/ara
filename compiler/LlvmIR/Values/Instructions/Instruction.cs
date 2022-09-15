namespace LlvmIR.Values.Instructions;

public abstract class Instruction : NamedValue
{
    protected Instruction(Block block, string? name = null) : base(block, name)
    {
    }
}