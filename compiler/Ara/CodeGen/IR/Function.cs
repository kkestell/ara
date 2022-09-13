using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

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
        foreach (var p in type.Parameters)
        {
            block.AddInstruction(new FunctionArgumentValue(block, new IntegerType(32), p.Name));
        }
        blocks.Add(block);
        return block;
    }

    public void Emit(StringBuilder sb)
    {
        sb.AppendLine($"define {type.ReturnType.ToIr()} @{name} ({string.Join(", ", type.Parameters.Select(x => x.ToIr()))}) {{");
        blocks.ForEach(x => x.Emit(sb));
        sb.AppendLine("}");
    }
}
