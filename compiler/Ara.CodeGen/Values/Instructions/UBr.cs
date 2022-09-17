using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values.Instructions;

public class UBr : Instruction
{
    readonly Label label;
    
    public UBr(Block block, Label label, string? name = null) : base(block, name)
    {
        this.label = label;
    }

    public override IrType Type => IrType.Void;
    
    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br label {label.Resolve()}");
    }
}