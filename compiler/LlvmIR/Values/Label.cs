using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values;

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
        sb.AppendLine($"{Name}:");
    }
}