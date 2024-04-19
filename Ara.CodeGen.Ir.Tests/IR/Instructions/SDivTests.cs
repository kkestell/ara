#region

using System;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.IR.Tests.IR.Instructions;

public class SDivTests : TestBase
{
    [Test]
    public void SignedDivideTwoIntegers()
    {
        Builder.SDiv(new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = sdiv i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.SDiv(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.SDiv(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}