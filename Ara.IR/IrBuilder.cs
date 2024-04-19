using Ara.IR.Instructions;
using Ara.IR.Types;

namespace Ara.IR;

public class IrBuilder
{
    private int _nextVar;
    private Dictionary<string, Value> _valueMap = new();

    public IrBuilder(Module? module = null)
    {
        Module = module ?? new Module();
    }

    public Module Module { get; }
    public Function? CurrentFunction { get; private set; }
    public BasicBlock? CurrentBlock { get; private set; }
    
    public void SetFunction(Function func)
    {
        CurrentFunction = func;
        _nextVar = func.ArgTypes.Count;
    }

    public void SetBlock(BasicBlock block)
    {
        CurrentBlock = block;
    }

    public string NewVariable()
    {
        var name = $"%{_nextVar}";
        _nextVar++;
        return name;
    }

    public BasicBlock AppendBasicBlock(string name)
    {
        var block = new BasicBlock(name);
        CurrentFunction?.AddBasicBlock(block);
        if (CurrentBlock == null)
        {
            SetBlock(block);
        }
        return block;
    }

    public IdentifiedStructType GetIdentifiedStruct(string name)
    {
        var @struct = Module.Structs.FirstOrDefault(s => s.Key == name).Value;
        if (@struct == null)
        {
            throw new ArgumentException($"Struct {name} not found");
        }
        return @struct;
    }
    
    public void SetValue(string name, Value value)
    {
        _valueMap[name] = value;
    }
    
    public Value GetValue(string name)
    {
        if (!_valueMap.ContainsKey(name))
        {
            throw new ArgumentException($"Value {name} not found");
        }
        return _valueMap[name];
    }
    
    public void Comment(string comment)
    {
        var inst = new Comment(comment);
        CurrentBlock?.Instructions.Add(inst);
    }
    
    public Value Load(Value address)
    {
        var result = address.IRType switch
        {
            PtrType ptrType => new Value(ptrType.Pointee, NewVariable()),
            PointerType pointerType => new Value(pointerType.Pointee, NewVariable()),
            _ => throw new ArgumentException("Invalid type for load operation")
        };
        var inst = new Load(address, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    public void Store(Value value, Value address)
    {
        var inst = new Store(value, address);
        CurrentBlock?.Instructions.Add(inst);
    }

    public Value? Call(IFunction func, Value[] args)
    {
        if (func.ReturnType is VoidType)
        {
            var inst = new Call(func, args.ToList());
            CurrentBlock?.Instructions.Add(inst);
            return null;
        }
        else
        {
            var result = new Value(func.ReturnType, NewVariable());
            var inst = new Call(func, args.ToList(), result);
            CurrentBlock?.Instructions.Add(inst);
            return result;
        }
    }
    
    public Value Add(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new Add(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FAdd(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new FAdd(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Sub(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new Sub(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FSub(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new FSub(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Mul(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new Mul(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FMul(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new FMul(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value SDiv(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new SDiv(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value UDiv(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new UDiv(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FDiv(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new FDiv(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value ICmp(string op, Value lhs, Value rhs)
    {
        var result = new Value(new BooleanType(), NewVariable());
        var inst = new ICmp(op, lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    public Value FCmp(string op, Value lhs, Value rhs)
    {
        var result = new Value(new BooleanType(), NewVariable());
        var inst = new FCmp(op, lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value ShiftLeft(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new ShiftLeft(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value ShiftRight(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new ShiftRight(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Or(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new Or(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value And(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new And(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Xor(Value lhs, Value rhs)
    {
        var result = new Value(lhs.IRType, NewVariable());
        var inst = new Xor(lhs, rhs, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    public void Br(Value condition, BasicBlock trueBlock, BasicBlock falseBlock)
    {
        var inst = new Br(condition, trueBlock, falseBlock);
        CurrentBlock?.Instructions.Add(inst);
    }
    
    public void Br(BasicBlock targetBlock)
    {
        var inst = new Br(targetBlock);
        CurrentBlock?.Instructions.Add(inst);
    }

    public void Ret(Value? value = null)
    {
        var inst = new Ret(value);
        CurrentBlock?.Instructions.Add(inst);
    }

    public Value Alloca(IrType irType)
    {
        var result = new Value(new PtrType(irType), NewVariable());
        var inst = new Alloca(irType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Gep(Value source, params Value[] indices)
    {
        var resultType = source.IRType;
        foreach (var index in indices)
        {
            if (resultType is PtrType ptrType)
            {
                resultType = ptrType.Pointee;
            }
            else if (resultType is PointerType pointerType)
            {
                resultType = pointerType.Pointee;
            }
            else if (resultType is LiteralStructType structType)
            {
                int fieldIndex = Convert.ToInt32(index.Constant);
                resultType = structType.Fields.Values.ToList()[fieldIndex];
            }
            else if (resultType is IdentifiedStructType identifiedStructType)
            {
                int fieldIndex = Convert.ToInt32(index.Constant);
                resultType = identifiedStructType.Fields.Values.ToList()[fieldIndex];
            }
            else if (resultType is ArrayType arrayType)
            {
                resultType = arrayType.ElementType;
            }
            else
            {
                throw new ArgumentException("Invalid type for GEP operation");
            }
        }
        var result = new Value(new PtrType(resultType), NewVariable());
        var inst = new Gep(source, indices.ToList(), result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value SIToFP(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new SIToFP(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    
    public Value FPToSI(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new FPToSI(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    
    public Value BitCast(Value source, IrType targetType)
    {   
        var result = new Value(targetType, NewVariable());
        var inst = new BitCast(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value Trunc(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new Trunc(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value ZExt(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new ZExt(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value SExt(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new SExt(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FPExt(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new FPExt(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
    
    public Value FPTrunc(Value source, IrType targetType)
    {
        var result = new Value(targetType, NewVariable());
        var inst = new FPTrunc(source, targetType, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }

    public Value Phi(List<(Value, BasicBlock)> incomingValues)
    {
        var resultType = incomingValues[0].Item1.IRType;
        var result = new Value(resultType, NewVariable());
        var inst = new Phi(incomingValues, result);
        CurrentBlock?.Instructions.Add(inst);
        return result;
    }
}