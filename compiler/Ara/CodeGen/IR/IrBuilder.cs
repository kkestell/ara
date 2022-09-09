using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Memory;
using Ara.CodeGen.IR.Values.Instructions.Operations;
using Ara.CodeGen.IR.Values.Instructions.Terminators;

namespace Ara.CodeGen.IR;

public class IrBuilder
{
    readonly Block block;
    
    public IrBuilder(Block block)
    {
        this.block = block;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void Return(Value value)
    {
        block.AddInstruction(new ReturnInstruction(value, block));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue Add(Value left, Value right, string? name = null)
    {
        return new AddInstruction(block, left, right, name);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue Sub(Value left, Value right, string? name = null)
    {
        return new SubInstruction(block, left, right, name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue Mul(Value left, Value right, string? name = null)
    {
        return new MulInstruction(block, left, right, name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue SDiv(Value left, Value right, string? name = null)
    {
        return new SDivInstruction(block, left, right, name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="size"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue Alloca(IrType type, int size = 1, string? name = null)
    {
        return block.AddInstruction(new AllocaInstruction(block, type, size, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pointer"></param>
    /// <param name="name"></param>
    public void Store(Value value, Value pointer, string? name = null)
    {
        block.AddInstruction(new StoreInstruction(block, value, pointer, name));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointer"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public NamedValue Load(NamedValue pointer, string? name = null)
    {
        return block.AddInstruction(new LoadInstruction(block, pointer, name));
    }
}