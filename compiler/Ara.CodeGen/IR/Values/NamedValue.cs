namespace Ara.CodeGen.IR.Values;

public abstract class NamedValue : Value
{
    protected NamedValue(Function function, string? name)
    {
        Function = function;
        
        if (name is not null)
            Name = function.RegisterName(name);
    }
    
    public Function Function { get; }

    public string? Name { get; private set; }

    public override string Resolve()
    {
        Name ??= Function.NextName();
        return $"%{Name}";
    }
}