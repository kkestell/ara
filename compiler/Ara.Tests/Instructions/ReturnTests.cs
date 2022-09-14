namespace Ara.Tests.Instructions;

public class ReturnTests : TestBase
{
    [Test]
    public void ReturnAnInteger()
    {
        builder.Return(new IntValue(1));
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
ret i32 1
}"));
    }
}