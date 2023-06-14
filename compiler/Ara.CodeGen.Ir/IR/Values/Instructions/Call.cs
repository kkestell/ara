#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Call : Instruction
{
    private readonly string _functionName;
    private readonly IEnumerable<Argument> _args;

    public override IrType Type { get; }

    public Call(Function function, string functionName, IrType returnType, IEnumerable<Argument> args, string? name = null) : base(function, name)
    {
        _functionName = functionName;
        Type = returnType;
        _args = args;
    }
    
    public override void Emit(StringBuilder sb)
    {
        var call = $"call {Type.ToIr()} @{_functionName}({string.Join(", ", _args.Select(a => a.ToIr()))})\n";
        
        if (Type is VoidType)
            sb.Append(call);
        else
            sb.Append($"{Resolve()} = {call}");
    }
}