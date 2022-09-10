namespace Ara.Tests.Instructions;

public class AllocaTests
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
    public void AllocateAnInteger()
    {
        builder.Alloca(intType);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n}"));
    }
}