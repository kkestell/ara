using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values.Instructions;

public class Call : Instruction
{
    readonly string functionName;
    readonly IEnumerable<Argument> args;

    public override IrType Type => IrType.Int32;

    public Call(Block block, string functionName, IEnumerable<Argument> args, string? name = null) : base(block, name)
    {
        this.functionName = functionName;
        this.args = args;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = call {Type.ToIr()} @{functionName}({string.Join(", ", args.Select(a => a.ToIr()))})\n");
    }
}