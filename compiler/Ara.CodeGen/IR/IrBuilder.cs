using Ara.CodeGen.Errors;
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
    
    public void GotoBlock(Block block, Action<IrBuilder> action)
    {
        var b = Block;
        Block = block;
        action.Invoke(this);
        Block = b;
    }

    public void GotoBlock(Block block)
    {
        Block = block;
    }
    
    public Value ResolveValue(Value value)
    {
        return value.Type is PointerType ? Load(value) : value;
    }

    public Label Label(string value)
    {
        return new Label(Block, value);
    }
    
    public void IfThen(Value predicate, Action<IrBuilder> thenAction)
    {
        var ifBlock = Block.AddChild("if");
        var endifBlock = Block.AddChild("endif");

        Br(predicate, ifBlock.Label, endifBlock.Label);
        
        GotoBlock(ifBlock, builder =>
        {
            thenAction.Invoke(builder);
            builder.Br(endifBlock.Label);
        });        
        
        GotoBlock(endifBlock);
    }

    public void IfElse(Value predicate, Action<IrBuilder> thenAction, Action<IrBuilder> elseAction)
    {
        var ifBlock = Block.AddChild("if");
        var elseBlock = Block.AddChild("else");
        var endifBlock = Block.AddChild("endif");

        Br(predicate, ifBlock.Label, elseBlock.Label);
        
        GotoBlock(ifBlock, builder =>
        {
            thenAction.Invoke(builder);
            builder.Br(endifBlock.Label);
        });
        
        GotoBlock(elseBlock, builder =>
        {
            elseAction.Invoke(builder);
            builder.Br(endifBlock.Label);
        });
        
        GotoBlock(endifBlock);
    }

    public void For(string counter, Value start, Value end, Action<IrBuilder, NamedValue> loop)
    {
        var forBlock = Block.AddChild("for");
        var endforBlock = Block.AddChild("endfor");
        
        // Init counter
        var cPtr = Alloca(new IntegerType(32));
        Store(start, cPtr);
        Br(forBlock.Label);
        
        // Loop body
        GotoBlock(forBlock);
        Load(cPtr, counter);
        loop.Invoke(this, cPtr);
        
        // Increment counter
        Store(Add(Load(cPtr), new IntegerValue(1)), cPtr);

        // Loop
        var cVal = Load(cPtr);
        Br(Icmp(IcmpCondition.SignedLessThan, cVal, end), forBlock.Label, endforBlock.Label);
        Br(endforBlock.Label);
        
        GotoBlock(endforBlock);
    }

    public GetElementPtr GetElementPtr(Value array, Value index, string? name = null)
    {
        return Block.AddInstruction(new GetElementPtr(Block, array, index, name));
    }

    public Phi Phi(Dictionary<Label, Value> values, string? name = null)
    {
        // FIXME: First instruction is label. Is this goofy?
        if (Block.InstructionCount > 1)
            throw new CodeGenException("Phi must be the first instruction in the basic block");
        
        return Block.AddInstruction(new Phi(Block, values, name));
    }

    public void Br(Label label)
    {
        Block.AddInstruction(new UBr(label));
    }

    public void Br(Value predicate, Label l1, Label l2)
    {
        Block.AddInstruction(new Br(predicate, l1, l2));
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

    public void Store(Value value, Value pointer)
    {
        Block.AddInstruction(new Store(value, pointer));
    }
    
    public Load Load(Value pointer, string? name = null)
    {
        return Block.AddInstruction(new Load(Block, pointer, name));
    }

    public Call Call(string functionName, IrType returnType, IEnumerable<Argument> arguments, string? name = null)
    {
        return Block.AddInstruction(new Call(Block, functionName, returnType, arguments, name));
    }

    public Select Select(Value cond, Value value1, Value value2, string? name = null)
    {
        return Block.AddInstruction(new Select(Block, cond, value1, value2, name));
    }
}