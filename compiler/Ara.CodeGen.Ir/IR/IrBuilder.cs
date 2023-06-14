#region

using Ara.CodeGen.Ir.Errors;
using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;
using Ara.CodeGen.Ir.IR.Values.Instructions;

#endregion

namespace Ara.CodeGen.Ir.IR;

public class IrBuilder
{
    public IrBuilder(Function function)
    {
        Function = function;
    }

    public Function Function { get; }
    
    public Value ResolveValue(Value value)
    {
        return value.Type is PointerType ? Load(value) : value;
    }

    public Label Label(string value)
    {
        return new Label(Function, value);
    }
    
    public void IfThen(Value predicate, Action<IrBuilder> thenAction)
    {
        var ifLabel = Label("if");
        var endifLabel = Label("endif");
        
        Br(predicate, ifLabel, endifLabel);

        Function.AddInstruction(ifLabel);
        
        thenAction.Invoke(this);
        
        if (Function.Instructions.Last() is not ReturnInstruction)
            Br(endifLabel);

        Function.AddInstruction(endifLabel);
    }

    public void IfElse(Value predicate, Action<IrBuilder> thenAction, Action<IrBuilder> elseAction)
    {
        var ifLabel = Label("if");
        var elseLabel = Label("else");
        var endifLabel = Label("endif");
        
        Br(predicate, ifLabel, elseLabel);
        
        Function.AddInstruction(ifLabel);

        thenAction.Invoke(this);
        
        if (Function.Instructions.Last() is not ReturnInstruction)
            Br(endifLabel);
        
        Function.AddInstruction(elseLabel);
        
        elseAction.Invoke(this);

        if (Function.Instructions.Last() is ReturnInstruction)
            return;
        
        Br(endifLabel);
        Function.AddInstruction(endifLabel);
    }

    public void For(string counterName, Value start, Value end, Action<IrBuilder, NamedValue> loop)
    {
        if (start is Alloca)
            start = Load(start);
        
        if (end is Alloca)
            end = Load(end);
        
        var condLabel = Label("for.cond");
        var bodyLabel = Label("for.body");
        var incLabel = Label("for.inc");
        var endLabel = Label("for.end");

        var counterPtr = Alloca(new IntegerType(32));
        Store(start, counterPtr);
        Br(condLabel);

        Function.AddInstruction(condLabel);
        var counterVal1 = Load(counterPtr, counterName);
        var compareResult = Icmp(IcmpCondition.SignedLessThan, counterVal1, end);
        Br(compareResult, bodyLabel, endLabel);

        Function.AddInstruction(bodyLabel);
        loop.Invoke(this, counterVal1);
        Br(incLabel);

        Function.AddInstruction(incLabel);
        var counterVal2 = Load(counterPtr);
        var newVal = Add(counterVal2, new IntegerValue(1));
        Store(newVal, counterPtr);
        Br(condLabel);

        Function.AddInstruction(endLabel);
    }

    public GetElementPtr GetElementPtr(Value array, Value index, string? name = null)
    {
        return Function.AddInstruction(new GetElementPtr(Function, array, index, name));
    }

    public Phi Phi(Dictionary<Label, Value> values, string? name = null)
    {
        // FIXME: First instruction is label. Is this goofy?
        if (Function.InstructionCount > 1)
            throw new CodeGenException("Phi must be the first instruction in the basic block");
        
        return Function.AddInstruction(new Phi(Function, values, name));
    }

    public void Br(Label label)
    {
        Function.AddInstruction(new UBr(label));
    }

    public void Br(Value predicate, Label l1, Label l2)
    {
        Function.AddInstruction(new Br(predicate, l1, l2));
    }
    
    public void Return(Value? value = null)
    {
        Function.AddInstruction(new ReturnInstruction(value, Function));
    }
    
    public Add Add(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new Add(Function, lhs, rhs, name));
    }
    
    public FAdd FAdd(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new FAdd(Function, lhs, rhs, name));
    }
    
    public Sub Sub(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new Sub(Function, lhs, rhs, name));
    }

    public FSub FSub(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new FSub(Function, lhs, rhs, name));
    }
    
    public Mul Mul(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new Mul(Function, lhs, rhs, name));
    }
    
    public FMul FMul(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new FMul(Function, lhs, rhs, name));
    }

    public SDiv SDiv(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new SDiv(Function, lhs, rhs, name));
    }
    
    public UDiv UDiv(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new UDiv(Function, lhs, rhs, name));
    }

    public FDiv FDiv(Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new FDiv(Function, lhs, rhs, name));
    }
    
    public Icmp Icmp(IcmpCondition condition, Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new Icmp(Function, condition, lhs, rhs, name));
    }
    
    public Fcmp Fcmp(FcmpCondition condition, Value lhs, Value rhs, string? name = null)
    {
        return Function.AddInstruction(new Fcmp(Function, condition, lhs, rhs, name));
    }

    public Alloca Alloca(IrType type, int size = 1, string? name = null)
    {
        return Function.AddInstruction(new Alloca(Function, type, size, name));
    }

    public void Store(Value value, Value pointer)
    {
        Function.AddInstruction(new Store(value, pointer));
    }
    
    public Load Load(Value pointer, string? name = null)
    {
        return Function.AddInstruction(new Load(Function, pointer, name));
    }

    public Call Call(string functionName, IrType returnType, IEnumerable<Argument> arguments, string? name = null)
    {
        return Function.AddInstruction(new Call(Function, functionName, returnType, arguments, name));
    }

    public Select Select(Value cond, Value value1, Value value2, string? name = null)
    {
        return Function.AddInstruction(new Select(Function, cond, value1, value2, name));
    }
}