using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public class Label: NamedValue
{
    public Label(Block block, string value) : base(block, value)
    {
    }
    
    public override IrType Type => IrType.Void;
    
    public override string Resolve()
    {
        return $"%\"{Name}\"";
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"\"{Name}\":");
    }
}