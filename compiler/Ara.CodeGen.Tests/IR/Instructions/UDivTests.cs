using System;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class UDivTests : TestBase
{
    [Test]
    public void UnsignedDivideTwoIntegers()
    {
        builder.UDiv(new IntegerValue(1), new IntegerValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = udiv i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.UDiv(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.UDiv(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}