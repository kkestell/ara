namespace LlvmIR.Tests.Instructions;

public class FDivTests : TestBase
{
    [Test]
    public void DivideTwoFloats()
    {
        builder.FDiv(new FloatValue(3.14f), new FloatValue(2.17f));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = fdiv float 0x40091EB860000000, 0x40015C2900000000
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FDiv(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotFloats()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.FDiv(new IntValue(1), new IntValue(1));
        });
    }
}