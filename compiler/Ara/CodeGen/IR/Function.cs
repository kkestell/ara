using System.Text;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class Function
{
    readonly string name;
    readonly FunctionType type;
    readonly List<Block> blocks = new();
    
    public Function(string name, FunctionType type)
    {
        this.name = name;
        this.type = type;
    }

    public int NumBlocks => blocks.Count;

    public Block AddBlock(Block block)
    {
        blocks.Add(block);
        return block;
    }
    
    public Block NewBlock()
    {
        var block = new Block(this);
        
        foreach (var p in type.Parameters)
        {
            block.AddInstruction(new ArgumentValue(block, IrType.Int, p.Name));
        }

        blocks.Add(block);
        
        return block;
    }

    public void Emit(StringBuilder sb)
    {
        var p = string.Join(", ", type.Parameters.Select(x => x.ToIr()));
        sb.AppendLine($"define {type.ReturnType.ToIr()} @{name} ({p}) {{");
        blocks.ForEach(x => x.Emit(sb));
        sb.AppendLine("}");
    }
}
