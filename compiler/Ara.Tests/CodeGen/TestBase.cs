namespace Ara.Tests.CodeGen;

public abstract class TestBase
{
    protected Module module;
    protected IrBuilder builder;

    [SetUp]
    public void Setup()
    {
        module = new Module();
        var func = module.AppendFunction("test", new FunctionType());
        var block = func.NewBlock();
        builder = block.Builder();
    }
    
    protected void AssertIr(string actual, string expected)
    {
        var trimmedActual = string.Join('\n', actual.Split('\n').Select(line => line.TrimStart())).Trim();
        var trimmedExpected = string.Join('\n', expected.Split('\n').Select(line => line.TrimStart())).Trim();
        Assert.That(trimmedActual, Is.EqualTo(trimmedExpected));
    }
}