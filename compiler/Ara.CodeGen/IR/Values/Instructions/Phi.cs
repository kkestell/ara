#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class Phi : Instruction
{
    private readonly Dictionary<Label, Value> _values;
    
    public Phi(Function function, Dictionary<Label, Value> values, string? name = null) : base(function, name)
    {
        if (values.Count == 0)
            throw new ArgumentException();
        
        _values = values;
    }

    public override IrType Type => _values.Values.First().Type;
    
    public override void Emit(StringBuilder sb)
    {
        var t = _values.Values.First().Type;
        var v = string.Join(", ", _values.Select(v => $"[{v.Value.Resolve()}, {v.Key.Resolve()}]"));
        sb.AppendLine($"{Resolve()} = phi {t.ToIr()} {v}");
    }
}