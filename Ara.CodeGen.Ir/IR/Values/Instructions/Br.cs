#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class Br : Value
{
    private readonly Value _predicate;
    private readonly Label _ifLabel;
    private readonly Label _endIfLabel;
    
    public Br(Value predicate, Label ifLabel, Label endIfLabel)
    {
        _predicate = predicate;
        _ifLabel = ifLabel;
        _endIfLabel = endIfLabel;
    }

    public override IrType Type => IrType.Void;

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br {_predicate.Type.ToIr()} {_predicate.Resolve()}, label {_ifLabel.Resolve()}, label {_endIfLabel.Resolve()}");
    }
}