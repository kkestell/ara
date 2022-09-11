
namespace Ara.Tests.Instructions;

public class IcmpTests : TestBase
{
    [Test]
    public void CompareTwoIntegersForEquality()
    {
        builder.Icmp(IcmpCondition.Equal, new IntegerValue(1), new IntegerValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = icmp eq i32 1, 1\n}"));
    }

    [Test]
    public void CompareTwoPointersForEquality()
    {
        var ptr1 = builder.Alloca(new IntegerType(32));
        var ptr2 = builder.Alloca(new IntegerType(32));
        builder.Icmp(IcmpCondition.Equal, ptr1, ptr2);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = alloca i32, align 4\n%\"2\" = icmp eq ptr %\"0\", %\"1\"\n}"));
    }

    [Test]
    public void CompareTwoIntegersForInequality()
    {
        builder.Icmp(IcmpCondition.NotEqual, new IntegerValue(1), new IntegerValue(1));

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = icmp ne i32 1, 1\n}"));
    }
    
    [Test]
    public void CompareTwoPointersForInequality()
    {
        var ptr1 = builder.Alloca(new IntegerType(32));
        var ptr2 = builder.Alloca(new IntegerType(32));
        builder.Icmp(IcmpCondition.NotEqual, ptr1, ptr2);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define void @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = alloca i32, align 4\n%\"2\" = icmp ne ptr %\"0\", %\"1\"\n}"));
    }
}