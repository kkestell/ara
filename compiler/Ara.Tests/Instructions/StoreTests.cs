namespace Ara.Tests.Instructions;

public class StoreTests : TestBase
{
    [Test]
    public void StoreAnInteger()
    {
        var ptr = builder.Alloca(IrType.Int);
        builder.Store(new IntValue(1), ptr);

        var ir = module.Emit();

        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = alloca i32, align 4
store i32 1, ptr %""0""
}"));
    }
}