namespace Ara.Tests;

public class NamedValueTests : TestBase
{
    [Test]
    public void UseTheProvidedValue()
    {
        var value = builder.Add(new IntValue(1), new IntValue(1), "foo");
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""foo"" = add i32 1, 1
ret i32 %""foo""
}"));
    }
}
