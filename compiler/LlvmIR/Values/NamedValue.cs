namespace LlvmIR.Values;

public abstract class NamedValue : Value
{
    protected NamedValue(Block block, string? name)
    {
        Name = block.RegisterName(name);
    }

    public string Name { get; }

    public override string Resolve()
    {
        return $"%\"{Name}\"";
    }
}