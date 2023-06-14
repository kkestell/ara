#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class UBr : Value
{
    private readonly Label _label;
    
    public UBr(Label label)
    {
        _label = label;
    }

    public override IrType Type => IrType.Void;

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br label {_label.Resolve()}");
    }
}