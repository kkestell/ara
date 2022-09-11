namespace Ara.Tests.Instructions;

public class SubTests : TestBase
{
    [Test]
    public void SubtractTwoIntegers()
    {
        builder.Sub(new IntegerValue(1), new IntegerValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = sub i32 1, 1\n}"));
    }
    
    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Sub(new IntegerValue(1), new FloatValue(3.14f));
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