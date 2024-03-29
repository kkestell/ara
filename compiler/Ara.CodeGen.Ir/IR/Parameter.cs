#region

using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR;

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