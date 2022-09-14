namespace Ara.Tests.Instructions;

public class AllocaTests : TestBase
{
    [Test]
    public void AllocateAnInteger()
    {
        builder.Alloca(IrType.Int);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = alloca i32, align 4
}"));
    }
}