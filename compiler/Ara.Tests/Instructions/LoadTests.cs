namespace Ara.Tests.Instructions;

public class LoadTests
{
    readonly IrType intType = new IntegerType(32);

    Module module;
    IrBuilder builder;

    [SetUp]
    public void Setup()
    {
        module = new Module();

        var func = module.AppendFunction("test", new FunctionType(intType));
        func.AppendBasicBlock();

        var block = func.AppendBasicBlock();

        builder = new IrBuilder(block);
    }

    [Test]
    public void LoadAnInteger()
    {
        var ptr = builder.Alloca(intType);
        var value = builder.Load(ptr);
        
        var ir = module.Emit();
        
        Assert.Multiple(() =>
        {
            Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = load i32, ptr %\"0\"\n}"));
            Assert.That(value.Type, Is.InstanceOf<IntegerType>());
        });
    }

    [Test]
    public void ThrowWhenArgumentIsNotAPointer()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            var ptr = builder.Alloca(intType);
            var value = builder.Load(ptr);
            builder.Load(value);
        });
    }
}