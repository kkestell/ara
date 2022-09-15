using System.Text;
using LlvmIR.Types;
using LlvmIR.Values;

namespace LlvmIR;

public class Function
{
    readonly string name;
    readonly FunctionType type;
    readonly List<Block> blocks = new();
    readonly Dictionary<string, ArgumentValue> arguments = new();
    
    public Function(string name, FunctionType type)
    {
        this.name = name;
        this.type = type;
    }

    public int NumBlocks => blocks.Count;

    public Block AddBlock()
    {
        var block = new Block(this);

        foreach (var p in type.Parameters)
        {
            arguments.Add(p.Name, new ArgumentValue(block, p.Type, p.Name));
        }
        
        blocks.Add(block);
        return block;
    }

    public Block AddBlock(Label label, Block parent)
    {
        var block = new Block(this, label, parent);
        blocks.Add(block);
        return block;
    }

    public Value? Argument(string name)
    {
        return !arguments.ContainsKey(name) ? null : arguments[name];
    }

    public void Emit(StringBuilder sb)
    {
        var p = string.Join(", ", type.Parameters.Select(x => x.ToIr()));
        sb.AppendLine($"define {type.ReturnType.ToIr()} @{name} ({p}) {{");
        blocks.ForEach(x => x.Emit(sb));
        sb.AppendLine("}");
    }
}
