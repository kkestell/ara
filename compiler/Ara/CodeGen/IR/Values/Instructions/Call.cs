using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Call : Instruction
{
    readonly string functionName;
    readonly IEnumerable<IR.Argument> args;

    public override IrType Type => new IntType(32);

    public Call(Block block, string functionName, IEnumerable<IR.Argument> args, string? name = null) : base(block, name)
    {
        this.functionName = functionName;
        this.args = args;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = call {Type.ToIr()} @{functionName}({string.Join(", ", args.Select(a => a.ToIr()))})\n");
    }
}