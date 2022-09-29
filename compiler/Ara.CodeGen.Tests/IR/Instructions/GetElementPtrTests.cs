using System;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class GetElementPtrTests : TestBase
{
    [Test]
    public void GetPointerToAnArrayElement()
    {
        var a = builder.Alloca(IrType.Integer, 5);
        var p = builder.GetElementPtr(a, new IntegerValue(1));
        builder.Return(p);

        var ir = module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %""0"" = alloca i32, i32 5, align 4
              %""1"" = getelementptr inbounds [5 x i32], ptr %""0"", i32 0, i32 1
              ret ptr %""1""
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}