using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public class FunctionArgumentValue : NamedValue
{
    public FunctionArgumentValue(Block block, IrType type, string? name) : base(block, name)
    {
        Type = type;
    }

    public override IrType Type { get; }
    
    public override void Emit(StringBuilder sb)
    {
    }
}