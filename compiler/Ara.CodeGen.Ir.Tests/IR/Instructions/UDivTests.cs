#region

using System;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class UDivTests : TestBase
{
    [Test]
    public void UnsignedDivideTwoIntegers()
    {
        Builder.UDiv(new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
                %0 = udiv i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.UDiv(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.UDiv(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}