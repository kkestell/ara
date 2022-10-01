using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Phi : Instruction
{
    readonly Dictionary<Label, Value> values;
    
    public Phi(Block block, Dictionary<Label, Value> values, string? name = null) : base(block, name)
    {
        if (values.Count == 0)
            throw new ArgumentException();
        
        this.values = values;
    }

    public override IrType Type => values.Values.First().Type;
    
    public override void Emit(StringBuilder sb)
    {
        var t = values.Values.First().Type;
        var v = string.Join(", ", values.Select(v => $"[{v.Value.Resolve()}, {v.Key.Resolve()}]"));
        sb.AppendLine($"{Resolve()} = phi {t.ToIr()} {v}");
    }
}