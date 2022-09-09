using Ara.CodeGen.IR.Values.Abstract;

namespace Ara.CodeGen.IR.Values.Instructions.Abstract;

public abstract class Instruction : NamedValue
{
    protected Instruction(Block block, string? name = null) : base(block, name)
    {
    }
}