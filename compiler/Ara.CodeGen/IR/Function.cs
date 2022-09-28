using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

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

    public Block AddBlock(Block parent, string blockName)
    {
        var block = new Block(this, parent, blockName);
        blocks.Add(block);
        return block;
    }

    public Value? Argument(string argName)
    {
        return !arguments.ContainsKey(argName) ? null : arguments[argName];
    }

    public void Emit(StringBuilder sb)
    {
        var p = string.Join(", ", type.Parameters.Select(x => x.ToIr()));
        sb.AppendLine($"define {type.ReturnType.ToIr()} @{name} ({p}) {{");
        blocks.ForEach(x => x.Emit(sb));
        sb.AppendLine("}");
    }
}
