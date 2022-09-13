namespace Ara.Tests.Instructions;

public class FcmpTests : TestBase
{
    [Test]
    public void CompareTwoFloatsForEquality()
    {
        builder.Fcmp(FcmpCondition.OrderedAndEqual, new FloatValue(3.14f), new FloatValue(2.71f));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = fcmp oeq float 0x40091EB860000000, 0x4005AE1480000000\n}"));
    }
}