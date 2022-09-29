using Ara.Ast.Nodes;
using Ara.CodeGen.Errors;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;
using Argument = Ara.CodeGen.IR.Argument;
using ArrayType = Ara.Ast.Semantics.Types.ArrayType;
using Block = Ara.Ast.Nodes.Block;
using BooleanType = Ara.Ast.Semantics.Types.BooleanType;
using Call = Ara.Ast.Nodes.Call;

namespace Ara.CodeGen;

public class CodeGenerator
{
    readonly Dictionary<string, FunctionType> functionTypes = new ();

    public string Generate(SourceFile root)
    {
        var module = new Module();

        foreach (var d in root.Definitions.Nodes)
        {
            if (d is not FunctionDefinition f)
                continue;
            
            functionTypes.Add(f.Name, FunctionType.FromDefinition(f));
        }
        
        foreach (var d in root.Definitions.Nodes)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    EmitFunction(module, f);
                    break;
            }
        }
        
        return module.Emit();
    }
    
    void EmitFunction(Module module, FunctionDefinition functionDefinition)
    {
        var type = functionTypes[functionDefinition.Name];
        var function = module.AddFunction(functionDefinition.Name, type);
        var block = function.AddBlock();
        var builder = block.IrBuilder();

        EmitBlock(builder, functionDefinition.Block);
    }
    
    void EmitBlock(IrBuilder builder, Block block)
    {
        foreach (var statement in block.Statements.Nodes)
        {
            switch (statement)
            {
                case Assignment a:
                {
                    EmitAssignment(builder, a);
                    break;
                }
                case ArrayAssignment a:
                {
                    EmitArrayAssignment(builder, a);
                    break;
                }
                case For f:
                {
                    EmitFor(builder, f);
                    break;
                }
                case If i:
                {
                    EmitIf(builder, i);
                    break;
                }
                case Return r:
                {
                    EmitReturn(builder, r);
                    break;
                }
                case VariableDeclaration v:
                {
                    EmitVariableDeclaration(builder, v);
                    break;
                }
            }
        }
    }

    void EmitAssignment(IrBuilder builder, Assignment a)
    {
        var val = EmitExpression(builder, a.Expression);
        var ptr = builder.Block.FindNamedValue<Alloca>(a.Name);
        builder.Store(val, ptr, a.Name);
    }
    
    void EmitArrayAssignment(IrBuilder builder, ArrayAssignment a)
    {
        var val = EmitExpression(builder, a.Expression);
        var idx = EmitExpression(builder, a.Index);
        var ptr = builder.Block.FindNamedValue(a.Name);
        var elp = builder.GetElementPtr(ptr, idx);
        builder.Store(val, elp);
    }
    
    void EmitFor(IrBuilder builder, For f)
    {
        var s = EmitExpression(builder, f.Start);
        var e = EmitExpression(builder, f.End);
        builder.For(f.Counter, s, e, (loop, cnt) => EmitBlock(loop.IrBuilder(), f.Block));
    }
    
    void EmitIf(IrBuilder builder, If i)
    {
        var predicate = EmitExpression(builder, i.Predicate);
        builder.IfThen(predicate, then => EmitBlock(then, i.Then));
    }
    
    void EmitReturn(IrBuilder builder, Return r)
    {
        var expr = EmitExpression(builder, r.Expression);
        var val = builder.ResolveValue(expr);
        builder.Return(val);
    }

    void EmitVariableDeclaration(IrBuilder builder, VariableDeclaration v)
    {
        Value ptr = v.Type switch
        {
            ArrayType a => builder.Alloca(IrType.FromType(a.Type), a.Size, v.Name),
            _           => builder.Alloca(IrType.FromType(v.Type), 1, v.Name)
        };

        if (v.Expression is null)
            return;
        
        var val = EmitExpression(builder, v.Expression);
        builder.Store(val, ptr);
    }
    
    Value EmitExpression(IrBuilder builder, Expression expression)
    {
        return expression switch
        {
            ArrayIndex        e => EmitArrayIndex(builder, e),
            BinaryExpression  e => EmitBinaryExpression(builder, e),
            Call              e => EmitCall(builder, e),
            Constant          e => MakeConstant(e),
            VariableReference e => EmitVariableReference(builder, e),
            
            _ => throw new CodeGenException($"Unsupported expression type {expression.GetType()}.")
        };
    }

    Value EmitArrayIndex(IrBuilder builder, ArrayIndex expression)
    {
        var ptr = EmitVariableReference(builder, expression.VariableReference);
        var idx = EmitExpression(builder, expression.Index);
        return builder.GetElementPtr(ptr, idx);
    }

    Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    {
        var left  = builder.ResolveValue(EmitExpression(builder, expression.Left));
        var right = builder.ResolveValue(EmitExpression(builder, expression.Right));

        if (!left.Type.Equals(right.Type))
            throw new CodeGenException($"Binary expression types {left.Type.ToIr()} and {right.Type.ToIr()} don't match.");

        if (left.Type.GetType() == typeof(IntegerType))
        {
            return expression.Op switch
            {
                BinaryOperator.Add        => builder.Add(left, right),
                BinaryOperator.Subtract   => builder.Sub(left, right),
                BinaryOperator.Multiply   => builder.Mul(left, right),
                BinaryOperator.Divide     => builder.SDiv(left, right),
                // FIXME: Modulo
                
                BinaryOperator.Equality   => builder.Icmp(IcmpCondition.Equal, left, right),
                BinaryOperator.Inequality => builder.Icmp(IcmpCondition.NotEqual, left, right),

                _ => throw new CodeGenException($"Unsupported unary operator {expression.Op.ToString()}.")
            };
        }
        
        if (left.Type.GetType() == typeof(FloatType))
        {
            return expression.Op switch
            {
                BinaryOperator.Add        => builder.FAdd(left, right),
                BinaryOperator.Subtract   => builder.FSub(left, right),
                BinaryOperator.Multiply   => builder.FMul(left, right),
                BinaryOperator.Divide     => builder.FDiv(left, right),
                // FIXME: Modulo

                BinaryOperator.Equality   => builder.Fcmp(FcmpCondition.OrderedAndEqual, left, right),
                BinaryOperator.Inequality => builder.Fcmp(FcmpCondition.OrderedAndNotEqual, left, right),
                
                _ => throw new CodeGenException($"Unsupported binary operator {expression.Op.ToString()}.")
            };   
        }

        throw new CodeGenException($"Unsupported binary operand type {left.Type.ToIr()}");
    }

    Value EmitCall(IrBuilder builder, Call call)
    {
        var name = call.Name;

        if (!functionTypes.ContainsKey(name))
            throw new Exception($"No such function `{name}`");
        
        var functionType = functionTypes[call.Name];

        var args = new List<Argument>();
        foreach (var arg in call.Arguments.Nodes)
        {
            var param = functionType.Parameters.SingleOrDefault(x => x.Name == arg.Name);
            
            if (param is null)
                throw new CodeGenException($"Function {name} has no parameter {arg.Name}");

            var val = EmitExpression(builder, arg.Expression);

            if (!val.Type.Equals(param.Type))
                throw new CodeGenException($"Invalid argument type {val.Type.ToIr()} where {param.Type.ToIr()} was expected.");
            
            args.Add(new Argument(param.Type, val));
        }
        
        return builder.Call(call.Name, functionType.ReturnType, args);
    }

    static Value MakeConstant(Constant constant)
    {
        return constant.Type switch
        {
            Ast.Semantics.Types.IntegerType => new IntegerValue(int.Parse(constant.Value)),
            Ast.Semantics.Types.FloatType   => new FloatValue(float.Parse(constant.Value)),
            BooleanType => new BooleanValue(bool.Parse(constant.Value)),
                
            _ => throw new CodeGenException($"A constant of type {constant.Type} is not supported here.")
        };
    }

    static Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    {
        return builder.Block.FindNamedValue(reference.Name);
    }
}
