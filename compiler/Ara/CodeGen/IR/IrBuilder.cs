using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.IR;

public class IrBuilder
{
    Block block;
    
    public IrBuilder(Block block)
    {
        this.block = block;
    }

    public void IfThen(Value predicate, Action<Block> then)
    {
        var l1 = $"if.{block.Function.NumBlocks}";
        var l2 = $"if.{block.Function.NumBlocks + 1}";
        
        Br(predicate, l1, l2);
        var thenBlock = block.Function.AddBlock(l1, block);
        then.Invoke(thenBlock);
        block = block.Function.AddBlock(l2, block);
    }

    public void Br(Value predicate, string l1, string l2)
    {
        block.AddInstruction(new Br(block, predicate, l1, l2));
    }
    
    public void Return(Value value)
    {
        block.AddInstruction(new ReturnInstruction(value, block));
    }
    
    public Add Add(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Add(block, left, right, name));
    }
    
    public FAdd FAdd(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new FAdd(block, left, right, name));
    }
    
    public Sub Sub(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Sub(block, left, right, name));
    }

    public FSub FSub(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new FSub(block, left, right, name));
    }
    
    public Mul Mul(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Mul(block, left, right, name));
    }
    
    public FMul FMul(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new FMul(block, left, right, name));
    }

    public SDiv SDiv(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new SDiv(block, left, right, name));
    }
    
    public UDiv UDiv(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new UDiv(block, left, right, name));
    }

    public FDiv FDiv(Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new FDiv(block, left, right, name));
    }
    
    public Icmp Icmp(IcmpCondition condition, Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Icmp(block, condition, left, right, name));
    }
    
    public Fcmp Fcmp(FcmpCondition condition, Value left, Value right, string? name = null)
    {
        return block.AddInstruction(new Fcmp(block, condition, left, right, name));
    }

    public Alloca Alloca(IrType type, int size = 1, string? name = null)
    {
        return block.AddInstruction(new Alloca(block, type, size, name));
    }

    public void Store(Value value, Value pointer, string? name = null)
    {
        block.AddInstruction(new Store(block, value, pointer, name));
    }
    
    public Load Load(NamedValue pointer, string? name = null)
    {
        return block.AddInstruction(new Load(block, pointer, name));
    }

    public Value NamedValue(string name)
    {
        return block.NamedValue(name);
    }

    public Call Call(string functionName, IEnumerable<Argument> arguments, string? name = null)
    {
        return block.AddInstruction(new Call(block, functionName, arguments, name));
    }
}