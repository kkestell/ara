namespace Ara.CodeGen.IR.Values;

public abstract class NamedValue : Value
{
    protected NamedValue(Block block, string? name)
    {
        Name = block.Scope.Register(name);
    }

    public string Name { get; }

    public override string Resolve()
    {
        return $"%\"{Name}\"";
    }
}