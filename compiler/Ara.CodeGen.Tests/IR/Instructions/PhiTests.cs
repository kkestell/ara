using System;
using System.Collections.Generic;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class PhiTests : TestBase
{
    [Test]
    public void PhiOnePair()
    {
        var values = new Dictionary<Label, Value>
        {
            { new Label(builder.Block, "test1"), new IntegerValue(1) }
        };
        builder.Phi(values);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = phi i32 [1, %""test1""]
            }
        ");
    }
    
    [Test]
    public void PhiTwoPairs()
    {
        var values = new Dictionary<Label, Value>
        {
            { new Label(builder.Block, "test1"), new IntegerValue(1) },
            { new Label(builder.Block, "test2"), new IntegerValue(2) }
        };
        builder.Phi(values);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = phi i32 [1, %""test1""], [2, %""test2""]
            }
        ");
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