#region

using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.CodeGen.Errors;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;
using Argument = Ara.CodeGen.IR.Argument;
using ArrayType = Ara.Ast.Types.ArrayType;
using BooleanValue = Ara.Ast.Nodes.Expressions.Values.BooleanValue;
using Call = Ara.Ast.Nodes.Expressions.Call;
using ExternalFunctionDeclaration = Ara.Ast.Nodes.ExternalFunctionDeclaration;
using FloatValue = Ara.Ast.Nodes.Expressions.Values.FloatValue;
using IntegerValue = Ara.Ast.Nodes.Expressions.Values.IntegerValue;
using VoidType = Ara.Ast.Types.VoidType;

#endregion

namespace Ara.CodeGen;

public class CodeGenerator
{
    private readonly Dictionary<string, FunctionType> _functionTypes = new ();

    public string Generate(SourceFile root)
    {
        var module = new Module();

        if (root.ExternalFunctionDeclarations is not null)
        {
            foreach (var f in root.ExternalFunctionDeclarations.Nodes)
            {
                _functionTypes.Add(f.Name, FunctionType.FromExternalDeclaration(f));
            }
        }
        
        foreach (var f in root.FunctionDefinitions.Nodes)
        {
            _functionTypes.Add(f.Name, FunctionType.FromDefinition(f));
        }

        if (root.ExternalFunctionDeclarations is not null)
        {
            foreach (var f in root.ExternalFunctionDeclarations.Nodes)
            {
                EmitExternalFunctionDeclaration(module, f);
            }
        }
        
        foreach (var f in root.FunctionDefinitions.Nodes)
        {
            EmitFunction(module, f);
        }
        
        return module.Emit();
    }

    private void EmitExternalFunctionDeclaration(Module module, ExternalFunctionDeclaration externalFunctionDeclaration)
    {
        module.DeclareExternalFunction(
            new IR.ExternalFunctionDeclaration(
                externalFunctionDeclaration.Name,
                IrType.FromType(externalFunctionDeclaration.Type),
                externalFunctionDeclaration.Parameters.Nodes.Select(x => IrType.FromType(x.Type)).ToList()));
    }

    private void EmitFunction(Module module, FunctionDefinition functionDefinition)
    {
        var type = _functionTypes[functionDefinition.Name];
        var function = module.AddFunction(functionDefinition.Name, type);
        var builder = function.IrBuilder();
        
        EmitBlock(builder, functionDefinition.Block);

        if (functionDefinition.Type is VoidType)
        {
            builder.Return();
        }
    }

    private void EmitBlock(IrBuilder builder, Block block)
    {
        foreach (var statement in block.Statements.Nodes)
        {
            EmitStatement(builder, statement);
        }
    }

    private void EmitStatement(IrBuilder builder, Statement s)
    {
        switch (s)
        {
            case Assignment a:
                EmitAssignment(builder, a);
                break;
            case ArrayAssignment a:
                EmitArrayAssignment(builder, a);
                break;
            case Call c:
                EmitCall(builder, c);
                break;
            case Block b:
                EmitBlock(builder, b);
                break;
            case For f:
                EmitFor(builder, f);
                break;
            case If i:
                EmitIf(builder, i);
                break;
            case IfElse i:
                EmitIfElse(builder, i);
                break;
            case Return r:
                EmitReturn(builder, r);
                break;
            case VariableDeclaration v:
                EmitVariableDeclaration(builder, v);
                break;
        }
    }

    private void EmitAssignment(IrBuilder builder, Assignment a)
    {
        var val = EmitExpression(builder, a.Expression);
        var ptr = builder.Function.FindNamedValue<Alloca>(a.Name);
        builder.Store(val, ptr);
    }

    private void EmitArrayAssignment(IrBuilder builder, ArrayAssignment a)
    {
        var val = EmitExpression(builder, a.Expression);
        var idx = EmitExpression(builder, a.Index);
        var ptr = builder.Function.FindNamedValue(a.Name);
        var elp = builder.GetElementPtr(ptr, idx);
        builder.Store(val, elp);
    }

    private void EmitFor(IrBuilder builder, For f)
    {
        var s = EmitExpression(builder, f.Start);
        var e = EmitExpression(builder, f.End);
        builder.For(f.Counter, s, e, (loop, _) => EmitBlock(loop, f.Block));
    }

    private void EmitIf(IrBuilder builder, If i)
    {
        var predicate = EmitExpression(builder, i.Predicate);
        builder.IfThen(
            predicate,
            thenBuilder => EmitStatement(thenBuilder, i.Then));
    }

    private void EmitIfElse(IrBuilder builder, IfElse i)
    {
        var predicate = EmitExpression(builder, i.Predicate);
        var val = builder.ResolveValue(predicate);
        builder.IfElse(
            val,
            thenBuilder => EmitStatement(thenBuilder, i.Then),
            elseBuilder => EmitStatement(elseBuilder, i.Else));
    }

    private void EmitReturn(IrBuilder builder, Return r)
    {
        var expr = EmitExpression(builder, r.Expression);
        var val = builder.ResolveValue(expr);
        builder.Return(val);
    }

    private void EmitVariableDeclaration(IrBuilder builder, VariableDeclaration v)
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

    private Value EmitExpression(IrBuilder builder, Expression expression)
    {
        return expression switch
        {
            ArrayIndex        e => EmitArrayIndex(builder, e),
            BinaryExpression  e => EmitBinaryExpression(builder, e),
            Call              e => EmitCall(builder, e),
            IntegerValue      e => MakeInteger(e),
            FloatValue        e => MakeFloat(e),
            BooleanValue      e => MakeBoolean(e),
            VariableReference e => EmitVariableReference(builder, e),
            
            _ => throw new CodeGenException($"Unsupported expression type {expression.GetType()}.")
        };
    }

    private Value EmitArrayIndex(IrBuilder builder, ArrayIndex expression)
    {
        var ptr = EmitVariableReference(builder, expression.VariableReference);
        var idx = EmitExpression(builder, expression.Index);
        return builder.GetElementPtr(ptr, idx);
    }

    private Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    {
        var left  = builder.ResolveValue(EmitExpression(builder, expression.Left));
        var right = builder.ResolveValue(EmitExpression(builder, expression.Right));

        if (!left.Type.Equals(right.Type))
            throw new CodeGenException($"Binary expression types {left.Type.ToIr()} and {right.Type.ToIr()} don't match.");

        if (left.Type is IntegerType or BooleanType)
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

    private Value EmitCall(IrBuilder builder, Call call)
    {
        var name = call.Name;

        if (!_functionTypes.ContainsKey(name))
            throw new Exception($"No such function `{name}`");
        
        var functionType = _functionTypes[call.Name];

        var args = new List<Argument>();
        foreach (var arg in call.Arguments.Nodes)
        {
            var param = functionType.Parameters.SingleOrDefault(x => x.Name == arg.Name);
            
            if (param is null)
                throw new CodeGenException($"Function {name} has no parameter {arg.Name}");

            var val = EmitExpression(builder, arg.Expression);

            if (val is Alloca alloca)
            {
                val = builder.Load(alloca);
            }
            
            if (!val.Type.Equals(param.Type))
                throw new CodeGenException($"Invalid argument type {val.Type.ToIr()} where {param.Type.ToIr()} was expected.");
            
            args.Add(new Argument(param.Type, val));
        }
        
        return builder.Call(call.Name, functionType.ReturnType, args);
    }

    private static Value MakeInteger(IntegerValue constant) => new IR.Values.IntegerValue(constant.Value);

    private static Value MakeFloat(FloatValue constant) => new IR.Values.FloatValue(constant.Value);

    private static Value MakeBoolean(BooleanValue constant) => new IR.Values.BooleanValue(constant.Value);

    private static Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    {
        return builder.Function.FindNamedValue(reference.Name);
    }
}
