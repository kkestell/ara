namespace Ara.Tests.Instructions;

public class AddTests : TestBase
{
    [Test]
    public void AddTwoIntegers()
    {
        builder.Add(new IntValue(1), new IntValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = add i32 1, 1
}"));
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}