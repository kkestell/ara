namespace Ara.Tests;

public class NamedValueTests : TestBase
{
    [Test]
    public void UseTheProvidedValue()
    {
        var value = builder.Add(new IntegerValue(1), new IntegerValue(1), "foo");
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"foo\" = add i32 1, 1\nret i32 %\"foo\"\n}"));
    }
}