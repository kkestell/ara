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

    public Block AddBlock(string blockName, Block? parent = null)
    {
        var block = new Block(blockName, this, parent);
        
        if (parent is null)
        {
            foreach (var p in type.Parameters)
            {
                block.AddInstruction(new ArgumentValue(block, new IntType(32), p.Name));
            }
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
