using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class UBr : Value
{
    readonly Label label;
    
    public UBr(Label label)
    {
        this.label = label;
    }

    public override IrType Type => IrType.Void;

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br label {label.Resolve()}");
    }
}