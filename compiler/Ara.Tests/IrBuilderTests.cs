namespace Ara.Tests;

public class IrBuilderTests : TestBase
{
    [Test]
    public void IfThen()
    {
        var predicate = builder.Icmp(IcmpCondition.Equal, new IntValue(1), new IntValue(1));
        builder.IfThen(predicate, (block) =>
        {
            var thenBuilder = new IrBuilder(block);
            thenBuilder.Add(new IntValue(1), new IntValue(1));
        });

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = icmp eq i32 1, 1
br i1 %""0"", label %""if.1"", label %""if.2""
if.1:
%""0"" = add i32 1, 1
if.2:
}"));

    }
}
