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
    
    public Value ResolveValue(Value value)
    {
        if (value is Alloca a)
        {
            return Load(a);
        }

        return value;
    }

    public Label Label(string value)
    {
        return new Label(Block, value);
    }
    
    public void IfThen(Value predicate, Action<Block> then)
    {
        var l1 = new Label(Block, "if");
        var l2 = new Label(Block, "endif");
        
        Br(predicate, l1, l2);
        var thenBlock = Block.AddChild(l1);
        then.Invoke(thenBlock);
        Block = Block.AddChild(l2);
    }

    public void IfElse(Value predicate, Action<Block> thenAction, Action<Block> elseAction)
    {
        var l1 = new Label(Block, "if");
        var l2 = new Label(Block, "else");
        var l3 = new Label(Block, "endif");
        
        Br(predicate, l1, l2);
        var thenBlock = Block.AddChild(l1);
        thenAction.Invoke(thenBlock);
        Block = thenBlock;
        Br(l3);
        var elseBlock = Block.AddChild(l2);
        elseAction.Invoke(elseBlock);
        Block = elseBlock;
        Br(l3);
        Block = Block.AddChild(l3);
    }

    public void For(string counter, Value start, Value end, Action<Block, NamedValue> loop)
    {
        var l1 = new Label(Block, "for");
        var l2 = new Label(Block, "endfor");

        // Loop direction
        var dp = Icmp(IcmpCondition.SignedGreaterThan, end, start);
        var delta = Select(dp, new IntegerValue(1), new IntegerValue(-1));

        // Init counter
        var c = Alloca(new IntegerType(32));
        Store(start, c);
        Br(l1);

        // Loop body
        var b = Block.AddChild(l1);
        b.IrBuilder().Load(c, counter);
        loop.Invoke(b, c);
        Block = b;
        
        // Update counter
        Store(Add(Load(c), delta), c);

        var foo = Load(c);

        // Loop or end
        IfElse(
            dp,
            up => {
                var upBuilder = up.IrBuilder();
                var upP = upBuilder.Icmp(IcmpCondition.SignedLessThan, foo, end);
                upBuilder.Br(upP, l1, l2);
            },
            down => {
                var downBuilder = down.IrBuilder();
                var downP = downBuilder.Icmp(IcmpCondition.SignedGreaterThan, foo, end);
                downBuilder.Br(downP, l1, l2);
            }
        );

        Br(l2);
        Block = Block.AddChild(l2);
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
        Block.AddInstruction(new UBr(Block, label));
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

    public Alloca Alloca(IrType type, string? name = null)
    {
        return Block.AddInstruction(new Alloca(Block, type, name));
    }

    public void Store(Value value, Value pointer, string? name = null)
    {
        Block.AddInstruction(new Store(Block, value, pointer, name));
    }
    
    public Load Load(NamedValue pointer, string? name = null)
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