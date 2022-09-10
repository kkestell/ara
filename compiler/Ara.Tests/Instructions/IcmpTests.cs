namespace Ara.Tests.Instructions;

public class IcmpTests
{
    Module module;
    IrBuilder builder;

    [SetUp]
    public void Setup()
    {
        module = new Module();

        var func = module.AppendFunction("test", new FunctionType(new IntegerType(32)));
        func.AppendBasicBlock();

        var block = func.AppendBasicBlock();

        builder = new IrBuilder(block);
    }

    [Test]
    public void CompareTwoIntegersForEquality()
    {
        var value = builder.Icmp(IcmpCondition.Equal, new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = icmp eq i32 1, 1\nret i1 %\"0\"\n}"));
    }

    [Test]
    public void CompareTwoPointersForEquality()
    {
        var ptr1 = builder.Alloca(new IntegerType(32));
        var ptr2 = builder.Alloca(new IntegerType(32));
        var value = builder.Icmp(IcmpCondition.Equal, ptr1, ptr2);
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = alloca i32, align 4\n%\"2\" = icmp eq ptr %\"0\", %\"1\"\nret i1 %\"2\"\n}"));
    }

    [Test]
    public void CompareTwoIntegersForInequality()
    {
        var value = builder.Icmp(IcmpCondition.NotEqual, new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = icmp ne i32 1, 1\nret i1 %\"0\"\n}"));
    }
    
    [Test]
    public void CompareTwoPointersForInequality()
    {
        var ptr1 = builder.Alloca(new IntegerType(32));
        var ptr2 = builder.Alloca(new IntegerType(32));
        var value = builder.Icmp(IcmpCondition.NotEqual, ptr1, ptr2);
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = alloca i32, align 4\n%\"2\" = icmp ne ptr %\"0\", %\"1\"\nret i1 %\"2\"\n}"));
    }
}