namespace Ara.Tests.Instructions;

public class BrTests : TestBase
{
    [Test]
    public void Branch()
    {
        var predicate = builder.Icmp(IcmpCondition.Equal, new IntValue(1), new IntValue(1));
        builder.Br(predicate, "l1", "l2");

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo(@"define void @test () {
entry:
%""0"" = icmp eq i32 1, 1
br i1 %""0"", label %""l1"", label %""l2""
}"));
    }
}