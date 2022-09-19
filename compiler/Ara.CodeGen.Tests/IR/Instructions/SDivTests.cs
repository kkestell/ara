using System;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class SDivTests : TestBase
{
    [Test]
    public void SignedDivideTwoIntegers()
    {
        builder.SDiv(new IntegerValue(1), new IntegerValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = sdiv i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.SDiv(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.SDiv(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}