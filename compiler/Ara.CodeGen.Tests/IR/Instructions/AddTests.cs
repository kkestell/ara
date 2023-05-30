#region

using System;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class AddTests : TestBase
{
    [Test]
    public void AddTwoIntegers()
    {
        Builder.Add(new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = add i32 1, 1
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Add(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}