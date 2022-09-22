using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Call : Instruction
{
    readonly string functionName;
    readonly IEnumerable<Argument> args;

    public override IrType Type { get; }

    public Call(Block block, string functionName, IrType returnType, IEnumerable<Argument> args, string? name = null) : base(block, name)
    {
        this.functionName = functionName;
        Type = returnType;
        this.args = args;
    }
    
    public override void Emit(StringBuilder sb)
    {
        sb.Append($"{Resolve()} = call {Type.ToIr()} @{functionName}({string.Join(", ", args.Select(a => a.ToIr()))})\n");
    }
}