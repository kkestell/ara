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
        builder.IfThen(new BooleanValue(true), thenBlock =>
        {
            var thenBuilder = thenBlock.IrBuilder();
            thenBuilder.Add(new IntegerValue(1), new IntegerValue(1));
        });
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                br i1 1, label %""if"", label %""endif""
            if:
                %""1"" = add i32 1, 1
            endif:
            }
        ");
    }

    [Test]
    public void IfElse()
    {
        builder.IfElse(
            new BooleanValue(true),
            thenBlock =>
            {
                var thenBuilder = thenBlock.IrBuilder();
                thenBuilder.Return(new IntegerValue(1));
            },
            elseBlock =>
            {
                var elseBuilder = elseBlock.IrBuilder();
                elseBuilder.Return(new IntegerValue(1));
            });
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                br i1 1, label %""if"", label %""else""
            if:
                ret i32 1
                br label %""endif""
            else:
                ret i32 1
                br label %""endif""
            endif:
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
                var loopBuilder = loop.IrBuilder();
                loopBuilder.Return(loopBuilder.Load(counter));
            });

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = icmp sgt i32 10, 0
              %""1"" = select i1 %""0"", i32 1, i32 -1
              %""2"" = alloca i32, align 4
              store i32 0, ptr %""2""
              br label %""for""
            for:
              %""c"" = load i32, ptr %""2""
              %""5"" = load i32, ptr %""2""
              ret i32 %""5""
              %""7"" = load i32, ptr %""2""
              %""8"" = add i32 %""7"", %""1""
              store i32 %""8"", ptr %""2""
              %""10"" = load i32, ptr %""2""
              br i1 %""0"", label %""if"", label %""else""
            if:
              %""12"" = icmp slt i32 %""10"", 10
              br i1 %""12"", label %""for"", label %""endfor""
              br label %""endif""
            else:
              %""15"" = icmp sgt i32 %""10"", 10
              br i1 %""15"", label %""for"", label %""endfor""
              br label %""endif""
            endif:
              br label %""endfor""
            endfor:
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
