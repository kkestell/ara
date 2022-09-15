using System;
using LlvmIR.Values;

namespace LlvmIR.Tests.Instructions;

public class AddTests : TestBase
{
    [Test]
    public void AddTwoIntegers()
    {
        builder.Add(new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = add i32 1, 1
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntValue(1), new FloatValue(3.14f));
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