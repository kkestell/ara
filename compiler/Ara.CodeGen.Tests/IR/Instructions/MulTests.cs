using System;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class MulTests : TestBase
{
    [Test]
    public void MultiplyTwoIntegers()
    {
        builder.Mul(new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = mul i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Mul(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Mul(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}