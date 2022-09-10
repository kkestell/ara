namespace Ara.Tests.Instructions;

public class FAddTests
{
    Module module;
    IrBuilder builder;

    [SetUp]
    public void Setup()
    {
        module = new Module();

        var func = module.AppendFunction("test", new FunctionType(new FloatType()));
        func.AppendBasicBlock();

        var block = func.AppendBasicBlock();

        builder = new IrBuilder(block);
    }

    [Test]
    public void AddTwoFloats()
    {
        var value = builder.FAdd(new FloatValue(3.14f), new FloatValue(2.71f));
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define float @test () {\n%\"0\" = fadd float 0x40091EB860000000, 0x4005AE1480000000\nret float %\"0\"\n}"));
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}