using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Errors;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.Parsing.Nodes;

namespace Ara.CodeGen.IR;

public class CodeGenerator
{
    private readonly Dictionary<string, FunctionType> _functionTypes = new ();
    // private readonly HashSet<string> _structTypes = new ();

    public string Generate(CompilationUnit compilationUnit)
    {
        var module = new Module();

        // if (root.ExternalFunctionDeclarations is not null)
        // {
        //     foreach (var f in root.ExternalFunctionDeclarations.Nodes)
        //     {
        //         _functionTypes.Add(f.Name, FunctionType.FromExternalDeclaration(f));
        //     }
        // }
        
        foreach (var d in compilationUnit.Definitions)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    _functionTypes.Add(f.Identifier.Value, FunctionType.FromDefinition(f));
                    break;
                // case Parsing.Nodes.StructDeclaration s:
                //     _structTypes.Add(s.Identifier.Value);
                //     break;
            }
        }

        // if (compilationUnit.ExternalFunctionDeclarations is not null)
        // {
        //     foreach (var f in compilationUnit.ExternalFunctionDeclarations.Nodes)
        //     {
        //         EmitExternalFunctionDeclaration(module, f);
        //     }
        // }
        
        foreach (var d in compilationUnit.Definitions)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    EmitFunction(module, f);
                    break;
                // case Parsing.Nodes.StructDeclaration s:
                //     EmitStruct(module, s);
                //     break;
            }
        }
        
        return module.Emit();
    }

    // private static void EmitExternalFunctionDeclaration(Module module, ExternalFunctionDeclaration externalFunctionDeclaration)
    // {
    //     module.DeclareExternalFunction(
    //         new IR.ExternalFunctionDeclaration(
    //             externalFunctionDeclaration.Name,
    //             IrType.FromType(externalFunctionDeclaration.Type),
    //             externalFunctionDeclaration.Parameters.Nodes.Select(x => IrType.FromType(x.Type)).ToList()));
    // }

    private void EmitFunction(Module module, FunctionDefinition functionDefinition)
    {
        var type = _functionTypes[functionDefinition.Identifier.Value];
        var function = module.AddFunction(functionDefinition.Identifier.Value, type);
        var builder = function.IrBuilder();
        
        EmitStatement(builder, functionDefinition.Body);

        // if (functionDefinition.Type is VoidType)
        // {
        //     builder.Return();
        // }
    }

    // private static void EmitStruct(Module module, Parsing.Nodes.StructDeclaration structDefinition)
    // {
    //     module.AddStruct(structDefinition.Identifier.Value, structDefinition.Fields.Select(x => new StructField(x.Identifier.Value, IrType.FromType(x.Type))));
    // }

    private void EmitBlock(IrBuilder builder, Block block)
    {
        foreach (var statement in block.Statements)
        {
            EmitStatement(builder, statement);
        }
    }

    private void EmitStatement(IrBuilder builder, Statement s)
    {
        switch (s)
        {
        //     case Assignment a:
        //         EmitAssignment(builder, a);
        //         break;
        //     case ArrayAssignment a:
        //         EmitArrayAssignment(builder, a);
        //         break;
        //     case CallStatement c:
        //         EmitCallStatement(builder, c);
        //         break;
        case Block b:
            EmitBlock(builder, b);
            break;
        //     case For f:
        //         EmitFor(builder, f);
        //         break;
        //     case If i:
        //         EmitIf(builder, i);
        //         break;
        //     case IfElse i:
        //         EmitIfElse(builder, i);
        //         break;
        //     case Return r:
        //         EmitReturn(builder, r);
        //         break;
        case VariableDeclaration v:
            EmitVariableDeclaration(builder, v);
            break;
        }
    }

    // private void EmitAssignment(IrBuilder builder, Assignment a)
    // {
    //     var val = EmitExpression(builder, a.Expression);
    //     var ptr = builder.Function.FindNamedValue<Alloca>(a.Name);
    //     builder.Store(val, ptr);
    // }
    //
    // private void EmitArrayAssignment(IrBuilder builder, ArrayAssignment a)
    // {
    //     var val = EmitExpression(builder, a.Expression);
    //     var idx = EmitExpression(builder, a.Index);
    //     var ptr = builder.Function.FindNamedValue(a.Name);
    //     var elp = builder.GetElementPtr(ptr, idx);
    //     builder.Store(val, elp);
    // }
    //
    // private void EmitFor(IrBuilder builder, For f)
    // {
    //     var s = EmitExpression(builder, f.Start);
    //     var e = EmitExpression(builder, f.End);
    //     builder.For(f.Counter, s, e, (loop, _) => EmitBlock(loop, f.Block));
    // }
    //
    // private void EmitIf(IrBuilder builder, If i)
    // {
    //     var predicate = EmitExpression(builder, i.Predicate);
    //     builder.IfThen(
    //         predicate,
    //         thenBuilder => EmitStatement(thenBuilder, i.Then));
    // }
    //
    // private void EmitIfElse(IrBuilder builder, IfElse i)
    // {
    //     var predicate = EmitExpression(builder, i.Predicate);
    //     var val = builder.ResolveValue(predicate);
    //     builder.IfElse(
    //         val,
    //         thenBuilder => EmitStatement(thenBuilder, i.Then),
    //         elseBuilder => EmitStatement(elseBuilder, i.Else));
    // }
    //
    // private void EmitReturn(IrBuilder builder, Return r)
    // {
    //     var expr = EmitExpression(builder, r.Expression);
    //     var val = builder.ResolveValue(expr);
    //     builder.Return(val);
    // }
    
    private void EmitVariableDeclaration(IrBuilder builder, VariableDeclaration v)
    {
        Value ptr = v.TypeRef switch
        {
            // FIXME: Support multidimensional arrays
            Parsing.Nodes.ArrayTypeRef a => builder.Alloca(IrType.FromType(a.ElementType), a.Sizes, v.Identifier.Value),
            _                         => builder.Alloca(IrType.FromType(v.TypeRef), null, v.Identifier.Value)
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
            // ArrayIndex        e => EmitArrayIndex(builder, e),
            // BinaryExpression  e => EmitBinaryExpression(builder, e),
            // CallExpression    e => EmitCallExpression(builder, e),
            IntegerLiteral      e => MakeInteger(e),
            // FloatValue        e => MakeFloat(e),
            // BooleanValue      e => MakeBoolean(e),
            // VariableReference e => EmitVariableReference(builder, e),
            ArrayInitializationExpression e => EmitArrayInitializationExpression(builder, e),
            
            _ => throw new CodeGenException($"Unsupported expression type {expression.GetType()}.")
        };
    }

    private Value EmitArrayInitializationExpression(IrBuilder builder, ArrayInitializationExpression arrayInitializationExpression)
    {
        /*
        ========================================================================
        int main() {
            int x[2][3] = { { 1, 2, 3 }, { 4, 5, 6 } };
            return 0;
        }
        ========================================================================
        @__const.main.x = private unnamed_addr constant [2 x [3 x i32]] [[3 x i32] [i32 1, i32 2, i32 3], [3 x i32] [i32 4, i32 5, i32 6]], align 16

        ; Function Attrs: mustprogress noinline norecurse nounwind optnone uwtable
        define dso_local noundef i32 @main() #0 {
        entry:
          %retval = alloca i32, align 4
          %x = alloca [2 x [3 x i32]], align 16
          store i32 0, ptr %retval, align 4
          call void @llvm.memcpy.p0.p0.i64(ptr align 16 %x, ptr align 16 @__const.main.x, i64 24, i1 false)
          ret i32 0
        }

        ; Function Attrs: nocallback nofree nounwind willreturn memory(argmem: readwrite)
        declare void @llvm.memcpy.p0.p0.i64(ptr noalias nocapture writeonly, ptr noalias nocapture readonly, i64, i1 immarg) #1

        attributes #0 = { mustprogress noinline norecurse nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
        attributes #1 = { nocallback nofree nounwind willreturn memory(argmem: readwrite) }
        ========================================================================
        */

        throw new NotImplementedException();
    }

    //
    // private Value EmitArrayIndex(IrBuilder builder, ArrayIndex expression)
    // {
    //     var ptr = EmitVariableReference(builder, expression.VariableReference);
    //     var idx = EmitExpression(builder, expression.Index);
    //     return builder.GetElementPtr(ptr, idx);
    // }
    //
    // private Value EmitBinaryExpression(IrBuilder builder, BinaryExpression expression)
    // {
    //     var left  = builder.ResolveValue(EmitExpression(builder, expression.Left));
    //     var right = builder.ResolveValue(EmitExpression(builder, expression.Right));
    //
    //     if (!left.Type.Equals(right.Type))
    //         throw new CodeGenException($"Binary expression types {left.Type.ToIr()} and {right.Type.ToIr()} don't match.");
    //
    //     if (left.Type is IntegerType or BooleanType)
    //     {
    //         return expression.Op switch
    //         {
    //             BinaryOperator.Add        => builder.Add(left, right),
    //             BinaryOperator.Subtract   => builder.Sub(left, right),
    //             BinaryOperator.Multiply   => builder.Mul(left, right),
    //             BinaryOperator.Divide     => builder.SDiv(left, right),
    //             BinaryOperator.Modulo     => throw new NotImplementedException(),
    //             BinaryOperator.Equality   => builder.Icmp(IcmpCondition.Equal, left, right),
    //             BinaryOperator.Inequality => builder.Icmp(IcmpCondition.NotEqual, left, right),
    //
    //             _ => throw new CodeGenException($"Unsupported unary operator {expression.Op.ToString()}.")
    //         };
    //     }
    //     
    //     if (left.Type.GetType() == typeof(FloatType))
    //     {
    //         return expression.Op switch
    //         {
    //             BinaryOperator.Add        => builder.FAdd(left, right),
    //             BinaryOperator.Subtract   => builder.FSub(left, right),
    //             BinaryOperator.Multiply   => builder.FMul(left, right),
    //             BinaryOperator.Divide     => builder.FDiv(left, right),
    //             // FIXME: Modulo
    //
    //             BinaryOperator.Equality   => builder.Fcmp(FcmpCondition.OrderedAndEqual, left, right),
    //             BinaryOperator.Inequality => builder.Fcmp(FcmpCondition.OrderedAndNotEqual, left, right),
    //             
    //             _ => throw new CodeGenException($"Unsupported binary operator {expression.Op.ToString()}.")
    //         };   
    //     }
    //
    //     throw new CodeGenException($"Unsupported binary operand type {left.Type.ToIr()}");
    // }
    //
    // private Value EmitCallExpression(IrBuilder builder, CallExpression callExpression)
    // {
    //     var name = callExpression.Name;
    //     var args = BuildCallArguments(builder, name, callExpression.Arguments);
    //
    //     return builder.Call(name, _functionTypes[name].ReturnType, args);
    // }
    //
    // private void EmitCallStatement(IrBuilder builder, CallStatement callStatement)
    // {
    //     var name = callStatement.Name;
    //     var args = BuildCallArguments(builder, name, callStatement.Arguments);
    //
    //     builder.Call(name, _functionTypes[name].ReturnType, args);
    // }
    //
    // private IEnumerable<Argument> BuildCallArguments(IrBuilder builder, string name, NodeList<Ast.Nodes.Argument> callArguments)
    // {
    //     if (!_functionTypes.ContainsKey(name))
    //         throw new Exception($"No such function `{name}`");
    //
    //     var functionType = _functionTypes[name];
    //
    //     var args = new List<Argument>();
    //
    //     for (var i = 0; i < callArguments.Nodes.Count; i++)
    //     {
    //         var arg = callArguments.Nodes[i];
    //         var param = functionType.Parameters[i];
    //
    //         var val = EmitExpression(builder, arg.Expression);
    //
    //         if (val is Alloca alloca)
    //         {
    //             val = builder.Load(alloca);
    //         }
    //
    //         if (!val.Type.Equals(param.Type))
    //             throw new CodeGenException($"Invalid argument type {val.Type.ToIr()} where {param.Type.ToIr()} was expected.");
    //
    //         args.Add(new Argument(param.Type, val));
    //     }
    //
    //     return args;
    // }
    
    private static Value MakeInteger(IntegerLiteral constant) => new IntegerValue(constant.Value);
    
    // private static Value MakeFloat(FloatValue constant) => new IR.Values.FloatValue(constant.Value);
    //
    // private static Value MakeBoolean(BooleanValue constant) => new IR.Values.BooleanValue(constant.Value);
    //
    // private static Value EmitVariableReference(IrBuilder builder, VariableReference reference)
    // {
    //     return builder.Function.FindNamedValue(reference.Name);
    // }
}
