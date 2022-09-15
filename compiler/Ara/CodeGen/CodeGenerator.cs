using Ara.Ast.Nodes;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;
using Argument = Ara.CodeGen.IR.Argument;
using Block = Ara.Ast.Nodes.Block;
using Parameter = Ara.CodeGen.IR.Parameter;

namespace Ara.CodeGen;

public static class CodeGenerator
{
    public static string Generate(AstNode root)
    {
        if (root is not SourceFile sourceFile)
            throw new NotSupportedException();

        var module = new Module();

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
    
    static void EmitFunction(Module module, FunctionDefinition def)
    {
        var type = new FunctionType(
            IrType.FromString(def.ReturnType.Value),
            def.Parameters.Select(x =>
                new Parameter(x.Name.Value, IrType.FromString(x.Type.Value))).ToList());
        var function = module.AppendFunction(def.Name.Value, type);
        var block = function.NewBlock();
        var builder = block.Builder();

        EmitBlock(def.Block, builder);
    }
    
    static void EmitBlock(Block block, IrBuilder builder)
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
                    builder.IfThen(predicate, then => EmitBlock(i.Then, then.Builder()));
                    break;
                }
                case Assignment a:
                {
                    var val = EmitExpression(builder, a.Expression);
                    var ptr = builder.Block.NamedValue<Alloca>(a.Name.Value);
                    builder.Store(val, ptr, a.Name.Value);
                    break;
                }
            }
        }
    }

    static Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
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

    static Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    {
        return builder.Block.NamedValue(reference.Name.Value);
    }

    static Value EmitFunctionCallExpression(IrBuilder builder, Ast.Nodes.Call call)
    {
        var args = call.Arguments.Select(a => new Argument(
            // FIXME
            IrType.Int,
            builder.ResolveValue(EmitExpression(builder, a.Expression)))).ToList();
        return builder.Call(call.Name.Value, args);
    }
    
    static Value EmitExpression(IrBuilder builder, Expression expression)
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
