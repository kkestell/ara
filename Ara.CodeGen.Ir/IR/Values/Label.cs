#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values;

public class Label: NamedValue
{
    public Label(Function function, string value) : base(function, value)
    {
    }
    
    public override IrType Type => IrType.Void;
    
    public override string Resolve()
    {
        return $"%{Name}";
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"{Name}:");
    }
}