#region

using System;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class SubTests : TestBase
{
    [Test]
    public void SubtractTwoIntegers()
    {
        Builder.Sub(new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
                %0 = sub i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Sub(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Sub(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}