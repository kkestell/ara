using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values.Instructions;

public class Br : Instruction
{
    readonly Value predicate;
    readonly Label ifLabel;
    readonly Label endIfLabel;
    
    public Br(Block block, Value predicate, Label ifLabel, Label endIfLabel, string? name = null) : base(block, name)
    {
        this.predicate = predicate;
        this.ifLabel = ifLabel;
        this.endIfLabel = endIfLabel;
    }

    public override IrType Type => IrType.Void;
    
    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br {predicate.Type.ToIr()} {predicate.Resolve()}, label {ifLabel.Resolve()}, label {endIfLabel.Resolve()}");
    }
}