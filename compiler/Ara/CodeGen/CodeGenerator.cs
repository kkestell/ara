using Ara.Ast.Nodes;
using LlvmIR;
using LlvmIR.Types;
using LlvmIR.Values;
using LlvmIR.Values.Instructions;
using Argument = LlvmIR.Argument;
using Block = Ara.Ast.Nodes.Block;
using Parameter = LlvmIR.Parameter;

namespace Ara.CodeGen;

public class CodeGenerator
{
    readonly Dictionary<string, FunctionType> functionTypes = new ();

    FunctionType MakeFunctionType(FunctionDefinition def)
    {
        return new FunctionType(
            IrType.FromString(def.ReturnType.Value),
            def.Parameters.Select(x =>
                new Parameter(x.Name.Value, IrType.FromString(x.Type.Value))).ToList());
    }

    public string Generate(AstNode root)
    {
        if (root is not SourceFile sourceFile)
            throw new NotSupportedException();

        var module = new Module();

        CacheFunctionTypes(sourceFile.Definitions);

        foreach (var def in sourceFile.Definitions)
        {
            switch (def)
            {
                case FunctionDefinition f:
                    EmitFunction(module, f);
                    break;
            }
        }

        return module.Emit();
    }
    
    void CacheFunctionTypes(IEnumerable<Definition> defs)
    {
        foreach (var d in defs)
        {
            if (d is not FunctionDefinition f)
                continue;
            
            functionTypes.Add(f.Name.Value, MakeFunctionType(f));
        }
    }
    
    void EmitFunction(Module module, FunctionDefinition def)
    {
        var type = functionTypes[def.Name.Value];
        var function = module.AddFunction(def.Name.Value, type);
        var block = function.AddBlock();
        var builder = block.IrBuilder();

        EmitBlock(def.Block, builder);
    }
    
    void EmitBlock(Block block, IrBuilder builder)
    {
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case Return r:
                {
                    var expr = EmitExpression(builder, r.Expression);
                    var val = builder.ResolveValue(expr);
                    builder.Return(val);
                    break;
                }
                case VariableDeclaration v:
                {
                    var val = EmitExpression(builder, v.Expression);
                    var ptr = builder.Alloca(IrType.FromString(v.InferredType!.Value), 1, v.Name.Value);
                    builder.Store(val, ptr);
                    break;
                }
                case If i:
                {
                    var predicate = EmitExpression(builder, i.Predicate);
                    builder.IfThen(predicate, then => EmitBlock(i.Then, then.IrBuilder()));
                    break;
                }
                case Assignment a:
                {
                    var val = EmitExpression(builder, a.Expression);
                    var ptr = builder.Block.FindNamedValue<Alloca>(a.Name.Value);
                    builder.Store(val, ptr, a.Name.Value);
                    break;
                }
            }
        }
    }

    Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    {
        var left  = builder.ResolveValue(EmitExpression(builder, expression.Left));
        var right = builder.ResolveValue(EmitExpression(builder, expression.Right));

        if (!left.Type.Equals(right.Type))
            throw new ArgumentException();

        if (left.Type.GetType() == typeof(IntType))
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

                _ => throw new NotImplementedException()
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
                _ => throw new NotImplementedException()
            };   
        }

        throw new NotImplementedException();
    }

    Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    {
        return builder.Block.FindNamedValue(reference.Name.Value);
    }

    Value EmitFunctionCallExpression(IrBuilder builder, Ast.Nodes.Call call)
    {
        var name = call.Name.Value;

        if (!functionTypes.ContainsKey(name))
            throw new Exception($"No such function `{name}`");
        
        var functionType = functionTypes[call.Name.Value];

        var args = new List<Argument>();
        foreach (var a in call.Arguments)
        {
            var p = functionType.Parameters.SingleOrDefault(x => x.Name == a.Name.Value);
            
            if (p is null)
                throw new Exception($"Function {name} has no parameter {a.Name.Value}");

            var v = EmitExpression(builder, a.Expression);

            if (!v.Type.Equals(p.Type))
                throw new Exception($"Argument {a.Name.Value} got {v.Type.ToIr()} but expected {p.Type.ToIr()}");
            
            args.Add(new Argument(p.Type, v));
        }
        
        return builder.Call(call.Name.Value, args);
    }
    
    Value EmitExpression(IrBuilder builder, Expression expression)
    {
        if (expression is Constant c)
        {
            return c.Type.Value switch
            {
                "int"   => new IntValue(int.Parse(c.Value)),
                "float" => new FloatValue(float.Parse(c.Value)),
                
                _ => throw new NotImplementedException()
            };
        }
        
        return expression switch
        {
            BinaryExpression  e => EmitBinaryExpression(builder, e),
            VariableReference r => EmitVariableReference(builder, r),
            Ast.Nodes.Call    f => EmitFunctionCallExpression(builder, f),
            _ => throw new NotImplementedException()
        };
    }
}
