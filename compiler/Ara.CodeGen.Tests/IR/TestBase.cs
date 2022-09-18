using System.Linq;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR;

public abstract class TestBase
{
    protected Module module = null!;
    protected IrBuilder builder = null!;

    [SetUp]
    public void Setup()
    {
        module = new Module();
        var func = module.AddFunction("test", new FunctionType());
        var block = func.AddBlock();
        builder = block.IrBuilder();
    }

    protected static void AssertIr(string actual, string expected)
    {
        var a = Trim(actual);
        var e = Trim(expected);
        Assert.That(a, Is.EqualTo(e));
    }

    static string Trim(string str)
    {
        return string.Join('\n', str.Split('\n').Select(line => line.TrimStart())).Trim();
    }
}