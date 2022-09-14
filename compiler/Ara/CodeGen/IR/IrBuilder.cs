using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.IR;

public class IrBuilder
{
    public IrBuilder(Block block)
    {
        Block = block;
    }

    public Block Block { get; private set; }

    public Function Function => Block.Function;

    public Label Label(string value)
    {
        return new Label(Block, value);
    }
    
    public void IfThen(Value predicate, Action<Block> then)
    {
        var l1 = new Label(Block, "if");
        var l2 = new Label(Block, "endif");
        
        Br(predicate, l1, l2);
        //Block.AddInstruction(l1);
        var thenBlock = Block.AddChildBlock(l1);
        then.Invoke(thenBlock);
        Block = Block.AddChildBlock(l2);
        //Block.AddInstruction(l2);
    }

    public void Br(Value predicate, Label l1, Label l2)
    {
        Block.AddInstruction(new Br(Block, predicate, l1, l2));
    }
    
    public void Return(Value? value = null)
    {
        Block.AddInstruction(new ReturnInstruction(value, Block));
    }
    
    public Add Add(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new Add(Block, lhs, rhs, name));
    }
    
    public FAdd FAdd(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new FAdd(Block, lhs, rhs, name));
    }
    
    public Sub Sub(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new Sub(Block, lhs, rhs, name));
    }

    public FSub FSub(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new FSub(Block, lhs, rhs, name));
    }
    
    public Mul Mul(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new Mul(Block, lhs, rhs, name));
    }
    
    public FMul FMul(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new FMul(Block, lhs, rhs, name));
    }

    public SDiv SDiv(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new SDiv(Block, lhs, rhs, name));
    }
    
    public UDiv UDiv(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new UDiv(Block, lhs, rhs, name));
    }

    public FDiv FDiv(Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new FDiv(Block, lhs, rhs, name));
    }
    
    public Icmp Icmp(IcmpCondition condition, Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new Icmp(Block, condition, lhs, rhs, name));
    }
    
    public Fcmp Fcmp(FcmpCondition condition, Value lhs, Value rhs, string? name = null)
    {
        return Block.AddInstruction(new Fcmp(Block, condition, lhs, rhs, name));
    }

    public Alloca Alloca(IrType type, int size = 1, string? name = null)
    {
        return Block.AddInstruction(new Alloca(Block, type, size, name));
    }

    public void Store(Value value, Value pointer, string? name = null)
    {
        Block.AddInstruction(new Store(Block, value, pointer, name));
    }
    
    public Load Load(NamedValue pointer, string? name = null)
    {
        return Block.AddInstruction(new Load(Block, pointer, name));
    }

    public Call Call(string functionName, IEnumerable<Argument> arguments, string? name = null)
    {
        return Block.AddInstruction(new Call(Block, functionName, arguments, name));
    }
}