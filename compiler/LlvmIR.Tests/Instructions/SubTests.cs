namespace LlvmIR.Tests.Instructions;

public class SubTests : TestBase
{
    [Test]
    public void SubtractTwoIntegers()
    {
        builder.Sub(new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = sub i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Sub(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Sub(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}