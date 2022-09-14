namespace Ara.Tests.Instructions;

public class CallTests : TestBase
{
    [Test]
    public void Call()
    {
        builder.Call("test", new List<Argument> { new (new IntType(32), new IntValue(1))});

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = call i32 @test(i32 1)
}"));
    }
}