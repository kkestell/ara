using System;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class SelectTests : TestBase
{
    [Test]
    public void SelectAValue()
    {
        builder.Select(new BooleanValue(true), new IntegerValue(1), new IntegerValue(2));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = select i1 1, i32 1, i32 2
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Select(new BooleanValue(true), new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenConditionIsNotABoolean()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Select(new IntegerValue(1), new IntegerValue(2), new IntegerValue(3));
        });
    }
}