using System;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class FSubTests : TestBase
{
    [Test]
    public void SubtractTwoFloats()
    {
        builder.FSub(new FloatValue(3.14f), new FloatValue(2.71f));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = fsub float 0x40091EB860000000, 0x4005AE1480000000
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FSub(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotFloats()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FSub(new IntegerValue(1), new IntegerValue(1));
        });
    }
}