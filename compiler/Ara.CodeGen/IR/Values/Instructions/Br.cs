using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Br : Value
{
    readonly Value predicate;
    readonly Label ifLabel;
    readonly Label endIfLabel;
    
    public Br(Value predicate, Label ifLabel, Label endIfLabel)
    {
        this.predicate = predicate;
        this.ifLabel = ifLabel;
        this.endIfLabel = endIfLabel;
    }

    public override IrType Type => IrType.Void;

    public override string Resolve()
    {
        throw new NotImplementedException();
    }

    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br {predicate.Type.ToIr()} {predicate.Resolve()}, label {ifLabel.Resolve()}, label {endIfLabel.Resolve()}");
    }
}