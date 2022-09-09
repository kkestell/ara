using Ara.CodeGen.IR.Types.Abstract;

namespace Ara.CodeGen.IR;

public class Parameter
{
    readonly string name;
    readonly IrType type;

    public Parameter(string name, IrType type)
    {
        this.name = name;
        this.type = type;
    }

    public string ToIr() => $"{type.ToIr()} %{name}";
}
