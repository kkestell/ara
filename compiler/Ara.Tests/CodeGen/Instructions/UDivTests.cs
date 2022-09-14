namespace Ara.Tests.CodeGen.Instructions;

public class UDivTests : TestBase
{
    [Test]
    public void UnsignedDivideTwoIntegers()
    {
        builder.UDiv(new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = udiv i32 1, 1
            }
        ");
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.UDiv(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.UDiv(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}