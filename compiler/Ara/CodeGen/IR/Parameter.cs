using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

public class Parameter
{
    public Parameter(string name, IrType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    
    public IrType Type { get; }
    
    public string ToIr() => $"{Type.ToIr()} %{Name}";
}
