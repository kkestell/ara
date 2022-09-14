namespace Ara.Tests.Instructions;

public class UDivTests : TestBase
{
    [Test]
    public void UnsignedDivideTwoIntegers()
    {
        builder.UDiv(new IntValue(1), new IntValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = udiv i32 1, 1\n}"));
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