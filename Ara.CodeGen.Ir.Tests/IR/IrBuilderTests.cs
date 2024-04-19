using System.Collections.Generic;
using Ara.CodeGen.IR.Errors;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.IR.Tests.IR;

public class IrBuilderTests : TestBase
{
    #region Control Flow
    
    [Test]
    public void IfThen()
    {
        Builder.IfThen(new BooleanValue(true), then =>
        {
            then.Add(new IntegerValue(1), new IntegerValue(1));
        });

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              br i1 1, label %if, label %endif
            if:
              %0 = add i32 1, 1
              br label %endif
            endif:
            }"
        );
    }

    [Test]
    public void IfElse()
    {
        Builder.IfElse(
            new BooleanValue(true),
            thenBuilder =>
            {
                thenBuilder.Return(new IntegerValue(1));
            },
            elseBuilder =>
            {
                elseBuilder.Return(new IntegerValue(2));
            });

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              br i1 1, label %if, label %else
            if:
              ret i32 1
            else:
              ret i32 2
            }
        ");
    }

    [Test]
    public void For()
    {
        Builder.For(
            "c",
            new IntegerValue(0),
            new IntegerValue(10),
            (loop, counter) => {
            });

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %0 = alloca i32, i32 1, align 4
              store i32 0, ptr %0
              br label %for.cond
            for.cond:
              %c = load i32, ptr %0
              %1 = icmp slt i32 %c, 10
              br i1 %1, label %for.body, label %for.end
            for.body:
              br label %for.inc
              for.inc:
              %2 = load i32, ptr %0
              %3 = add i32 %2, 1
              store i32 %3, ptr %0
              br label %for.cond
            for.end:
            }
        ");
    }
    
    #endregion
    
    #region Functions
    
    [Test]
    public void SimplestFunction()
    {
        Builder.Return(new IntegerValue(42));

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              ret i32 42
            }
        ");
    }
    
    #endregion
    
    #region GetElementPtr

    [Test]
    public void GetElementPtr_CreatesCorrectInstruction()
    {
        var arrayPtr = Builder.Alloca(new ArrayType(IrType.Integer, new List<int> { 10 }));
        var indexVal = new IntegerValue(5);

        Builder.GetElementPtr(arrayPtr, indexVal);

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %0 = alloca {[10 x i32]}, i32 1, align 4
              %1 = getelementptr [10 x i32], ptr %0, i32 0, i32 5
            }
        ");
    }

    #endregion
    
    #region Br
    
    [Test]
    public void Br_Unconditional_CreatesCorrectInstruction()
    {
        var label = new Label(Builder.Function, "testLabel");

        Builder.Br(label);

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              br label %testLabel
            }
        ");
    }
    
    [Test]
    public void Br_Conditional_CreatesCorrectInstruction()
    {
        var predicate = new IntegerValue(1);
        var label1 = new Label(Builder.Function, "label1");
        var label2 = new Label(Builder.Function, "label2");

        Builder.Br(predicate, label1, label2);

        var ir = Module.Emit();
        AssertIr(ir, @"
        define void @test () {
        entry:
          br i32 1, label %label1, label %label2
        }
    ");
    }


    #endregion

    #region Phi

    [Test]
    public void Phi_CreatesCorrectInstruction()
    {
        var label1 = new Label(Builder.Function, "label1");
        var label2 = new Label(Builder.Function, "label2");
        var value1 = new IntegerValue(1);
        var value2 = new IntegerValue(2);
        var values = new Dictionary<Label, Value>()
        {
            { label1, value1 },
            { label2, value2 }
        };

        Builder.Phi(values, "phi1");

        var ir = Module.Emit();
        AssertIr(ir, @"
        define void @test () {
        entry:
          %phi1 = phi i32 [1, %label1], [2, %label2]
        }
    ");
    }
    
    [Test]
    public void ThrowWhenPhiIsNotTheFirstInstruction()
    {
        Module = new Module();

        var type = new FunctionType(IrType.Integer);
        var func = Module.AddFunction("main", type);
        
        Builder = func.IrBuilder();
        
        var values = new Dictionary<Label, Value>
        {
            { new Label(Builder.Function, "test1"), new IntegerValue(1) }
        };

        Builder.Add(new IntegerValue(1), new IntegerValue(2));
        
        Assert.Throws<CodeGenException>(delegate
        {
            Builder.Phi(values);
        });
    }
    
    #endregion

    [Test]
    public void ResolveValue_PointerTypeValue_LoadsValue()
    {
        Module = new Module();

        var type = new FunctionType(IrType.Integer);
        var func = Module.AddFunction("main", type);
    
        Builder = func.IrBuilder();

        var ptr = Builder.Alloca(IrType.Integer);
        var loadedValue = Builder.ResolveValue(ptr);

        Assert.IsTrue(loadedValue is Load);
    }
    
    [Test]
    public void Label_CreatesLabelWithFunction()
    {
        Module = new Module();
    
        var type = new FunctionType(IrType.Integer);
        var func = Module.AddFunction("main", type);
    
        Builder = func.IrBuilder();
    
        var label = Builder.Label("testLabel");
    
        Assert.AreEqual(label.Function, func);
        Assert.AreEqual(label.Name, "testLabel");
    }

    [Test]
    public void Alloca_CreatesAllocationInstruction()
    {
        Module = new Module();
    
        var type = new FunctionType(IrType.Integer);
        var func = Module.AddFunction("main", type);
    
        Builder = func.IrBuilder();
    
        var alloc = Builder.Alloca(new IntegerType(32));
    
        Assert.AreEqual(alloc.Type, new PointerType(new IntegerType(32)));
    }
}
