using Ara.IR.Types;

namespace Ara.IR.Instructions;

public abstract class InstructionBase
{
}

public class Comment : InstructionBase
{
    public string Text { get; }

    public Comment(string text)
    {
        Text = text;
    }

    public override string ToString() => $"; {Text}";
}

public class Call : InstructionBase
{
    public IFunction Function { get; }
    public List<Value> Args { get; }
    public Value? Result { get; }

    public Call(IFunction function, List<Value> args, Value? result = null)
    {
        Function = function;
        Args = args;
        Result = result;
    }

    public override string ToString()
    {
        var argsStr = string.Join(", ", Args.Select(arg => $"{arg.IRType} {arg.ConstantOrName}"));
        if (Result is null)
            return $"call {Function.ReturnType} @{Function.Name}({argsStr})";
        return $"{Result.Name} = call {Function.ReturnType} @{Function.Name}({argsStr})";
    }
}

public class Ret : InstructionBase
{
    public Value? Value { get; }

    public Ret(Value? value = null)
    {
        Value = value;
    }

    public override string ToString() => Value != null ? $"ret {Value}" : "ret void";
}

public class Alloca : InstructionBase
{
    public IrType AllocaType { get; }
    public Value Result { get; }

    public Alloca(IrType allocaType, Value result)
    {
        AllocaType = allocaType;
        Result = result;
    }

    public override string ToString()
    {
        return $"{Result.Name} = alloca {AllocaType}";
    }
}

public class Load : InstructionBase
{
    public Value Address { get; }
    public Value Result { get; }

    public Load(Value address, Value result)
    {
        Address = address;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = load {Result.IRType}, ptr {Address.ConstantOrName}";
}

public class Store : InstructionBase
{
    public Value Value { get; }
    public Value Address { get; }

    public Store(Value value, Value address)
    {
        Value = value;
        Address = address;
    }

    public override string ToString() => $"store {Value}, ptr {Address.ConstantOrName}";
}

public class Gep : InstructionBase
{
    public Value Source { get; }
    public List<Value> Indices { get; }
    public Value Result { get; }

    public Gep(Value source, List<Value> indices, Value result)
    {
        Source = source;
        Indices = indices;
        Result = result;
    }

    public override string ToString()
    {
        var indicesStr = string.Join(", ", Indices.Select(index => index.ToString()));
        
        if (Source.IRType is PtrType ptr)
            return $"{Result.Name} = getelementptr {ptr.Pointee}, ptr {Source.ConstantOrName}, {indicesStr}";
        
        if (Source.IRType is PointerType pointer)
            return $"{Result.Name} = getelementptr {pointer.Pointee}, ptr {Source.ConstantOrName}, {indicesStr}";
        
        throw new Exception($"Gep source must be a pointer, but was {Source.IRType}");
    }
}

public abstract class BinaryExpressionInstruction : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }
    public string Operator { get; }
    
    public BinaryExpressionInstruction(Value lhs, Value rhs, Value result, string op)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
        Operator = op;
    }

    public override string ToString() => $"{Result.Name} = {Operator} {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
    
}

public class Add : BinaryExpressionInstruction
{
    public Add(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "add")
    {
    }
}

public class FAdd : BinaryExpressionInstruction
{
    public FAdd(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "fadd")
    {
    }
}

public class Sub : BinaryExpressionInstruction
{
    public Sub(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "sub")
    {
    }
}

public class FSub : BinaryExpressionInstruction
{
    public FSub(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "fsub")
    {
    }
}

public class Mul : BinaryExpressionInstruction
{
    public Mul(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "mul")
    {
    }
}

public class FMul : BinaryExpressionInstruction
{
    public FMul(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "fmul")
    {
    }
}

public class SDiv : BinaryExpressionInstruction
{
    public SDiv(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "sdiv")
    {
    }
}

public class UDiv : BinaryExpressionInstruction
{
    public UDiv(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "udiv")
    {
    }
}

public class FDiv : BinaryExpressionInstruction
{
    public FDiv(Value lhs, Value rhs, Value result) : base(lhs, rhs, result, "fdiv")
    {
    }
}

public class ICmp : InstructionBase
{
    public string Op { get; }
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public ICmp(string op, Value lhs, Value rhs, Value result)
    {
        Op = op;
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = icmp {Op} {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class FCmp : InstructionBase
{
    public string Op { get; }
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public FCmp(string op, Value lhs, Value rhs, Value result)
    {
        Op = op;
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = fcmp {Op} {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class ShiftLeft : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public ShiftLeft(Value lhs, Value rhs, Value result)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = shl {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class ShiftRight : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public ShiftRight(Value lhs, Value rhs, Value result)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = lshr {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class Or : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public Or(Value lhs, Value rhs, Value result)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = or {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class And : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public And(Value lhs, Value rhs, Value result)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = and {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class Xor : InstructionBase
{
    public Value Lhs { get; }
    public Value Rhs { get; }
    public Value Result { get; }

    public Xor(Value lhs, Value rhs, Value result)
    {
        Lhs = lhs;
        Rhs = rhs;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = xor {Lhs.IRType} {Lhs.ConstantOrName}, {Rhs.ConstantOrName}";
}

public class Br : InstructionBase
{
    public Value? Condition { get; }
    public BasicBlock TrueBlock { get; }
    public BasicBlock? FalseBlock { get; }

    public Br(BasicBlock trueBlock)
    {
        TrueBlock = trueBlock;
    }
    
    public Br(Value condition, BasicBlock trueBlock, BasicBlock falseBlock)
    {
        Condition = condition;
        TrueBlock = trueBlock;
        FalseBlock = falseBlock;
    }

    public override string ToString()
    {
        if (Condition is not null && FalseBlock is not null) 
            return $"br {Condition}, label %{TrueBlock.Name}, label %{FalseBlock!.Name}";

        return $"br label %{TrueBlock.Name}";
    }
}

public class Jmp : InstructionBase
{
    public BasicBlock TargetBlock { get; }

    public Jmp(BasicBlock targetBlock)
    {
        TargetBlock = targetBlock;
    }

    public override string ToString() => $"jmp label %{TargetBlock.Name}";
}

public class Phi : InstructionBase
{
    public List<(Value, BasicBlock)> IncomingValues { get; }
    public Value Result { get; }

    public Phi(List<(Value, BasicBlock)> incomingValues, Value result)
    {
        IncomingValues = incomingValues;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = phi {Result.IRType} " + string.Join(", ", IncomingValues.Select(iv => $"[{iv.Item1}, %{iv.Item2}]"));
}

public class SIToFP : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }

    public SIToFP(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }

    public override string ToString() => $"{Result.Name} = sitofp {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class FPToSI : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public FPToSI(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = fptosi {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class BitCast : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public BitCast(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = bitcast {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class Trunc : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public Trunc(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = trunc {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class ZExt : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public ZExt(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = zext {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class SExt : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public SExt(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = sext {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class FPExt : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public FPExt(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = fpext {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}

public class FPTrunc : InstructionBase
{
    public Value Source { get; }
    public IrType TargetType { get; }
    public Value Result { get; }
    
    public FPTrunc(Value source, IrType targetType, Value result)
    {
        Source = source;
        TargetType = targetType;
        Result = result;
    }
    
    public override string ToString() => $"{Result.Name} = fptrunc {Source.IRType} {Source.ConstantOrName} to {TargetType}";
}