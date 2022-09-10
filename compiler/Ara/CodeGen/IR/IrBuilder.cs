using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.IR;

public enum IcmpCondition
{
    Equal,
    NotEqual,
    UnsignedGreaterThan,
    UnsignedGreaterOrEqual,
    UnsignedLessThan,
    UnsignedLessOrEqual,
    SignedGreaterThan,
    SignedGreaterOrEqual,
    SignedLessThan,
    SignedLessOrEqual
}

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
    public Add Add(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Add(block, left, right, name));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public FAdd FAdd(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new FAdd(block, left, right, name));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sub Sub(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Sub(block, left, right, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Mul Mul(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Mul(block, left, right, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public SDiv SDiv(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new SDiv(block, left, right, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Icmp Icmp(IcmpCondition condition, Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Icmp(block, condition, left, right, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="size"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Alloca Alloca(IrType type, int size = 1, string? name = null)
    {
        return block.AddInstruction(new Alloca(block, type, size, name));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pointer"></param>
    /// <param name="name"></param>
    public void Store(Value value, Value pointer, string? name = null)
    {
        block.AddInstruction(new Store(block, value, pointer, name));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointer"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Load Load(NamedValue pointer, string? name = null)
    {
        return block.AddInstruction(new Load(block, pointer, name));
    }
}