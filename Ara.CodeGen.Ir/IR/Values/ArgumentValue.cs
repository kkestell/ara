#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values;

public class ArgumentValue : NamedValue
{
    public ArgumentValue(Function function, IrType type, string? name) : base(function, name)
    {
        Type = type;
    }

    public override IrType Type { get; }
    
    public override void Emit(StringBuilder sb)
    {
    }
}
