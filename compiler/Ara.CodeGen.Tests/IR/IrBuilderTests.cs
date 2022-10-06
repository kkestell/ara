using System.Collections.Generic;
using Ara.CodeGen.Errors;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR;

public class IrBuilderTests : TestBase
{
    [Test]
    public void IfThen()
    {
        builder.IfThen(new BooleanValue(true), then =>
        {
            then.Add(new IntegerValue(1), new IntegerValue(1));
        });

        var ir = module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              br i1 1, label %""if.0"", label %""endif.0""
            if.0:
              %""1"" = add i32 1, 1
              br label %""endif.0""
            endif.0:
            }
        ");
    }

    [Test]
    public void IfElse()
    {
        builder.IfElse(
            new BooleanValue(true),
            thenBuilder =>
            {
                thenBuilder.Return(new IntegerValue(1));
            },
            elseBuilder =>
            {
                elseBuilder.Return(new IntegerValue(2));
            });

        var ir = module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              br i1 1, label %""if.0"", label %""else.0""
            if.0:
              ret i32 1
              br label %""endif.0""
            else.0:
              ret i32 2
              br label %""endif.0""
            endif.0:
            }
        ");
    }

    [Test]
    public void For()
    {
        builder.For(
            "c",
            new IntegerValue(0),
            new IntegerValue(10),
            (loop, counter) => {
                loop.Return(loop.Load(counter));
            });

        var ir = module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %""0"" = alloca i32, i32 1, align 4
              store i32 0, ptr %""0""
              br label %""for.0""
            for.0:
              %""c"" = load i32, ptr %""0""
              %""3"" = load i32, ptr %""0""
              ret i32 %""3""
              %""5"" = load i32, ptr %""0""
              %""6"" = add i32 %""5"", 1
              store i32 %""6"", ptr %""0""
              %""8"" = load i32, ptr %""0""
              %""9"" = icmp slt i32 %""8"", 10
              br i1 %""9"", label %""for.0"", label %""endfor.0""
              br label %""endfor.0""
            endfor.0:
            }
        ");
    }
    
    [Test]
    public void SimplestFunction()
    {
        module = new Module();

        var type = new FunctionType(IrType.Integer);
        var func = module.AddFunction("main", type);

        var block = func.AddBlock();

        builder = block.IrBuilder();
        builder.Return(new IntegerValue(42));

        AssertIr(module.Emit(), @"
            define i32 @main () {
            entry:
                ret i32 42
            }
        ");
    }
    
    [Test]
    public void ThrowWhenPhiIsNotTheFirstInstruction()
    {
        module = new Module();

        var type = new FunctionType(IrType.Integer);
        var func = module.AddFunction("main", type);

        var block = func.AddBlock();

        builder = block.IrBuilder();
        
        var values = new Dictionary<Label, Value>
        {
            { new Label(builder.Block, "test1"), new IntegerValue(1) }
        };

        builder.Add(new IntegerValue(1), new IntegerValue(2));
        
        Assert.Throws<CodeGenException>(delegate
        {
            builder.Phi(values);
        });
    }
}
