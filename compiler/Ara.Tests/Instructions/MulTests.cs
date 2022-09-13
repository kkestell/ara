﻿namespace Ara.Tests.Instructions;

public class MulTests : TestBase
{
    [Test]
    public void MultiplyTwoIntegers()
    {
        builder.Mul(new IntegerValue(1), new IntegerValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = mul i32 1, 1\n}"));
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Mul(new IntegerValue(1), new FloatValue(3.14f));
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