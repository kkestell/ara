using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values;

public class ArgumentValue : NamedValue
{
    public ArgumentValue(Block block, IrType type, string? name) : base(block, name)
    {
        Type = type;
    }

    public override IrType Type { get; }
    
    public override void Emit(StringBuilder sb)
    {
    }
}
