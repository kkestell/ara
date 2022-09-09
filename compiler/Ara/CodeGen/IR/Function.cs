using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

/// <summary>
/// https://llvm.org/docs/LangRef.html#functions
/// </summary>
public class Function
{
    readonly string name;
    readonly Module module;
    readonly FunctionType type;
    readonly List<Block> blocks = new();
    
    public Function(Module module, string name, FunctionType type)
    {
        this.module = module;
        this.name = name;
        this.type = type;
    }

    public Block AppendBasicBlock()
    {
        var block = new Block();
        blocks.Add(block);
        return block;
    }

    public void Emit(StringBuilder sb)
    {
        sb.Append($"define {type.ReturnType.ToIr()} @{name} ({string.Join(", ", type.Parameters.Select(x => x.ToIr()))}) {{\n");
        blocks.ForEach(x => x.Emit(sb));
        sb.Append('}');
    }
}
