#region

using System;
using System.Collections.Generic;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class PhiTests : TestBase
{
    [Test]
    public void PhiOnePair()
    {
        var values = new Dictionary<Label, Value>
        {
            { new Label(Builder.Function, "test1"), new IntegerValue(1) }
        };
        Builder.Phi(values);

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = phi i32 [1, %test1]
            }
        ");
    }
    
    [Test]
    public void PhiTwoPairs()
    {
        var values = new Dictionary<Label, Value>
        {
            { new Label(Builder.Function, "test1"), new IntegerValue(1) },
            { new Label(Builder.Function, "test2"), new IntegerValue(2) }
        };
        Builder.Phi(values);

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = phi i32 [1, %test1], [2, %test2]
            }
        ");
    }
    
    [Test]
    public void ThrowWhenValuesEmpty()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.Phi(new Dictionary<Label, Value>());
        });
    }
}