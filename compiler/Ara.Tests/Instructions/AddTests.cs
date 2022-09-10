namespace Ara.Tests.Instructions;

public class AddTests
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
    public void AddTwoIntegers()
    {
        var value = builder.Add(new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = add i32 1, 1\nret i32 %\"0\"\n}"));
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