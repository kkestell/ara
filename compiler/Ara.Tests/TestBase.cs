namespace Ara.Tests;

public abstract class TestBase
{
    protected Module module;
    protected IrBuilder builder;

    [SetUp]
    public void Setup()
    {
        module = new Module();
        var func = module.AppendFunction("test", new FunctionType());
        var block = func.AddBlock("entry");
        builder = block.Builder();
    }
}