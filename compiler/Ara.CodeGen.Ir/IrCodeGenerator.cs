#region

using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.CodeGen.Ir.Errors;
using Ara.CodeGen.Ir.IR;
using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;
using Ara.CodeGen.Ir.IR.Values.Instructions;
using Argument = Ara.CodeGen.Ir.IR.Argument;
using ArrayType = Ara.Ast.Types.ArrayType;
using BooleanValue = Ara.Ast.Nodes.Expressions.Values.BooleanValue;
using ExternalFunctionDeclaration = Ara.Ast.Nodes.ExternalFunctionDeclaration;
using FloatValue = Ara.Ast.Nodes.Expressions.Values.FloatValue;
using IntegerValue = Ara.Ast.Nodes.Expressions.Values.IntegerValue;
using StructField = Ara.CodeGen.Ir.IR.StructField;
using VoidType = Ara.Ast.Types.VoidType;

#endregion

namespace Ara.CodeGen.Ir;

public class IrCodeGenerator : ICodeGenerator
{
    private readonly Dictionary<string, FunctionType> _functionTypes = new ();
    private readonly HashSet<string> _structTypes = new ();

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
        
        foreach (var d in root.Definitions.Nodes)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    _functionTypes.Add(f.Name, FunctionType.FromDefinition(f));
                    break;
                case StructDefinition s:
                    _structTypes.Add(s.Name);
                    break;
            }
        }

        if (root.ExternalFunctionDeclarations is not null)
        {
            foreach (var f in root.ExternalFunctionDeclarations.Nodes)
            {
                EmitExternalFunctionDeclaration(module, f);
            }
        }
        
        foreach (var d in root.Definitions.Nodes)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    EmitFunction(module, f);
                    break;
                case StructDefinition s:
                    EmitStruct(module, s);
                    break;
            }
        }
        
        return module.Emit();
    }

    private static void EmitExternalFunctionDeclaration(Module module, ExternalFunctionDeclaration externalFunctionDeclaration)
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

    private static void EmitStruct(Module module, StructDefinition structDefinition)
    {
        module.AddStruct(structDefinition.Name, structDefinition.Fields.Nodes.Select(x => new StructField(x.Name, IrType.FromType(x.Type))));
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
            case CallStatement c:
                EmitCallStatement(builder, c);
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
            CallExpression    e => EmitCallExpression(builder, e),
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
                BinaryOperator.Modulo     => throw new NotImplementedException(),
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
    
    private Value EmitCallExpression(IrBuilder builder, CallExpression callExpression)
    {
        var name = callExpression.Name;
        var args = BuildCallArguments(builder, name, callExpression.Arguments);

        return builder.Call(name, _functionTypes[name].ReturnType, args);
    }

    private void EmitCallStatement(IrBuilder builder, CallStatement callStatement)
    {
        var name = callStatement.Name;
        var args = BuildCallArguments(builder, name, callStatement.Arguments);

        builder.Call(name, _functionTypes[name].ReturnType, args);
    }
    
    private IEnumerable<Argument> BuildCallArguments(IrBuilder builder, string name, NodeList<Ast.Nodes.Argument> callArguments)
    {
        if (!_functionTypes.ContainsKey(name))
            throw new Exception($"No such function `{name}`");

        var functionType = _functionTypes[name];

        var args = new List<Argument>();

        for (var i = 0; i < callArguments.Nodes.Count; i++)
        {
            var arg = callArguments.Nodes[i];
            var param = functionType.Parameters[i];

            var val = EmitExpression(builder, arg.Expression);

            if (val is Alloca alloca)
            {
                val = builder.Load(alloca);
            }

            if (!val.Type.Equals(param.Type))
                throw new CodeGenException($"Invalid argument type {val.Type.ToIr()} where {param.Type.ToIr()} was expected.");

            args.Add(new Argument(param.Type, val));
        }

        return args;
    }
    
    private static Value MakeInteger(IntegerValue constant) => new IR.Values.IntegerValue(constant.Value);

    private static Value MakeFloat(FloatValue constant) => new IR.Values.FloatValue(constant.Value);

    private static Value MakeBoolean(BooleanValue constant) => new IR.Values.BooleanValue(constant.Value);

    private static Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    {
        return builder.Function.FindNamedValue(reference.Name);
    }
}
