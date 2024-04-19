#region

using System;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.IR.Tests.IR.Instructions;

public class MulTests : TestBase
{
    [Test]
    public void MultiplyTwoIntegers()
    {
        Builder.Mul(new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
                %0 = mul i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Mul(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Mul(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}