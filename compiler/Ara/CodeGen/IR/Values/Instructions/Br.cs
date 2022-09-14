using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Br : Instruction
{
    readonly Value predicate;
    readonly string l1;
    readonly string l2;
    
    public Br(Block block, Value predicate, string l1, string l2, string? name = null) : base(block, name)
    {
        this.predicate = predicate;
        this.l1 = l1;
        this.l2 = l2;
    }

    public override IrType Type => new VoidType();
    
    public override void Emit(StringBuilder sb)
    {
        sb.AppendLine($"br {predicate.Type.ToIr()} {predicate.Resolve()}, label %\"{l1}\", label %\"{l2}\"");
    }
}