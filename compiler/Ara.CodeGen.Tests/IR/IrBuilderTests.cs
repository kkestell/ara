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
              %""delta"" = alloca i32, align 4
              br i1 %""0"", label %""if"", label %""else""
            if:
              store i32 1, ptr %""delta""
              br label %""endif""
            else:
              store i32 -1, ptr %""delta""
              br label %""endif""
            endif:
              %""6"" = alloca i32, align 4
              store i32 0, ptr %""6""
              br label %""for""
            for:
              %""c"" = load i32, ptr %""6""
              %""9"" = load i32, ptr %""6""
              ret i32 %""9""
              %""11"" = load i32, ptr %""6""
              %""12"" = load i32, ptr %""delta""
              %""13"" = add i32 %""11"", %""12""
              store i32 %""13"", ptr %""6""
              %""15"" = load i32, ptr %""6""
              br i1 %""0"", label %""if.0"", label %""else.0""
            if.0:
              %""17"" = icmp slt i32 %""15"", 10
              br i1 %""17"", label %""for"", label %""endfor""
              br label %""endif.0""
            else.0:
              %""20"" = icmp sgt i32 %""15"", 10
              br i1 %""20"", label %""for"", label %""endfor""
              br label %""endif.0""
            endif.0:
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
}
