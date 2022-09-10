namespace Ara.Tests.Instructions;

public class StoreTests
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
    public void StoreAnInteger()
    {
        var ptr = builder.Alloca(intType);
        builder.Store(new IntegerValue(1), ptr);

        var ir = module.Emit();

        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\nstore i32 1, ptr %\"0\"\n}"));
    }
}