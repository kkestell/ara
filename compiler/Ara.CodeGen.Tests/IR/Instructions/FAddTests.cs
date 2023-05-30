#region

using System;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class FAddTests : TestBase
{
    [Test]
    public void AddTwoFloats()
    {
        Builder.FAdd(new FloatValue(3.14f), new FloatValue(2.71f));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = fadd float 0x40091EB860000000, 0x4005AE1480000000
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.FAdd(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotFloats()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.FAdd(new IntegerValue(1), new IntegerValue(1));
        });
    }
}