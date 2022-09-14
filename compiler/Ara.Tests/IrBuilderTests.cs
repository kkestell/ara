namespace Ara.Tests;

public class IrBuilderTests : TestBase
{
    [Test]
    public void IfThen()
    {
        builder.IfThen(new BoolValue(true), (block) =>
        {
            var thenBuilder = new IrBuilder(block);
            thenBuilder.Add(new IntValue(1), new IntValue(1));
        });

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
br i1 1, label %""if.1"", label %""if.2""
if.1:
%""1"" = add i32 1, 1
if.2:
}"));
    }
}
