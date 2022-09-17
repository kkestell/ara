using Ara.CodeGen.Values;

namespace LlvmIR.Tests.Instructions;

public class FMulTests : TestBase
{
    [Test]
    public void SubtractTwoFloats()
    {
        builder.FMul(new FloatValue(3.14f), new FloatValue(2.71f));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = fmul float 0x40091EB860000000, 0x4005AE1480000000
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FMul(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotFloats()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FMul(new IntValue(1), new IntValue(1));
        });
    }
}