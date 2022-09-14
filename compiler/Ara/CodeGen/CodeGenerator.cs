using Ara.Ast.Nodes;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;
using Argument = Ara.CodeGen.IR.Argument;
using Block = Ara.Ast.Nodes.Block;

namespace Ara.CodeGen;

public static class CodeGenerator
{
    static void EmitBlock(Block block, IrBuilder builder)
    {
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case ReturnStatement returnStatement:
                {
                    var value = EmitExpression(builder, returnStatement.Expression);
                    builder.Return(value);
                    break;
                }
                case VariableDeclaration variableDeclarationStatement:
                {
                    var ptr = builder.Alloca(IrType.FromString(variableDeclarationStatement.InferredType.Value));
                    var value = EmitExpression(builder, variableDeclarationStatement.Expression);
                    builder.Store(value, ptr);
                    builder.Load(ptr, variableDeclarationStatement.Name.Value);
                    break;
                }
                case IfStatement ifStatement:
                {
                    var predicate = EmitExpression(builder, ifStatement.Predicate);
                    builder.IfThen(predicate, then =>
                    {
                        EmitBlock(ifStatement.Then, then.Builder());
                    });
                    break;
                }
            }
        }
    }
    
    public static string Generate(AstNode rootNode)
    {
        if (rootNode is not SourceFile sourceFile)
            throw new NotSupportedException();

        var module = new Module();

        foreach (var def in sourceFile.Definitions)
        {
            if (def is not FunctionDefinition funcDef) 
                continue;

            var funcType = new FunctionType(
                IrType.FromString(funcDef.ReturnType.Value),
                funcDef.Parameters.Select(x => 
                    new IR.Parameter(x.Name.Value, IrType.FromString(x.Type.Value))).ToList());
            
            var function = module.AppendFunction(
                funcDef.Name.Value,
                funcType);

            var block = function.NewBlock();
            var builder = block.Builder();
            
            EmitBlock(funcDef.Block, builder);
        }

        return module.Emit();
    }

    static Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    {
        var left  = EmitExpression(builder, expression.Left);
        var right = EmitExpression(builder, expression.Right);

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

    static Value EmitFunctionCallExpression(IrBuilder builder, CallExpression call)
    {
        return builder.Call(call.Name.Value, call.Arguments.Select(a => new Argument(IrType.Int, EmitExpression(builder, a.Expression))).ToList());
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
            BinaryExpression       e => EmitBinaryExpression(builder, e),
            VariableReference      r => EmitVariableReference(builder, r),
            CallExpression f => EmitFunctionCallExpression(builder, f),
            _ => throw new NotImplementedException()
        };
    }
}
