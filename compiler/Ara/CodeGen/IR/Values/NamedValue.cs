namespace Ara.CodeGen.IR.Values;

public abstract class NamedValue : Value
{
    readonly string name;
    
    protected NamedValue(Block block, string? name)
    {
        this.name = block.Scope.Register(name);
    }

    public override string Resolve()
    {
        return $"%\"{name}\"";
    }
}