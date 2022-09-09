using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Ast.Nodes.Statements;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Types.Abstract;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Abstract;

namespace Ara.CodeGen;

public static class CodeGenerator
{
    public static string Generate(AstNode rootNode)
    {
        if (rootNode is not SourceFile sourceFile)
            throw new NotSupportedException();

        var module = new Module();

        foreach (var def in sourceFile.Definitions)
        {
            if (def is not FunctionDefinition funcDef) 
                continue;
            
            var function = module.AppendFunction(
                funcDef.Name.Value,
                new FunctionType(
                    MakeType(funcDef.ReturnType),
                    funcDef.Parameters.Select(x => new IR.Parameter(x.Name.Value, MakeType(x.Type)))
                )
            );

            var block = function.AppendBasicBlock();
            var builder = new IrBuilder(block);

            foreach (var statement in funcDef.Block.Statements)
            {
                switch (statement)
                {
                    case ReturnStatement returnStatement:
                    {
                        var value = EmitExpression(builder, returnStatement.Expression);
                        builder.Return(value);
                        break;
                    }
                    case VariableDeclarationStatement variableDeclarationStatement:
                    {
                        var ptr = builder.Alloca(MakeType(variableDeclarationStatement.Type));
                        var value = EmitExpression(builder, variableDeclarationStatement.Expression);
                        builder.Store(value, ptr);
                        builder.Load(ptr, variableDeclarationStatement.Name.Value);
                        break;
                    }
                }
            }
        }

        return module.Emit();
    }
    
    static IrType MakeType(Type_ type)
    {
        return type.Value switch
        {
            "void"  => new VoidType(),
            "int"   => new IntegerType(32),
            "bool"  => new IntegerType(1),
            "float" => new FloatType(),
            _ => throw new NotImplementedException()
        };
    }

    static Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    {
        var left  = EmitExpression(builder, expression.Left);
        var right = EmitExpression(builder, expression.Right);
        
        return expression switch
        {
            AdditionExpression       => builder.Add(left, right),
            SubtractionExpression    => builder.Sub(left, right),
            MultiplicationExpression => builder.Mul(left, right),
            DivisionExpression       => builder.SDiv(left, right),
            _ => throw new NotImplementedException()
        };
    }
    
    static Value EmitExpression(IrBuilder builder, Expression expression)
    {
        return expression switch
        {
            Integer          i => new IntegerValue(int.Parse(i.Value)),
            Float            f => new FloatValue(float.Parse(f.Value)),
            BinaryExpression e => EmitBinaryExpression(builder, e),
            _ => throw new NotImplementedException()
        };
    }
}
