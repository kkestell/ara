using Ara.IR.Instructions;

public class BasicBlock
{
    public string Name { get; }
    public List<InstructionBase> Instructions { get; } = [];

    public BasicBlock(string name)
    {
        Name = name;
    }

    public override string ToString() => $"{Name}:\n" + string.Join("\n", Instructions.Select(instr => instr.ToString()));
}