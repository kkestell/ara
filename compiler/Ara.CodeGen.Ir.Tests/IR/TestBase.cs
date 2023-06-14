#region

using System.Linq;
using Ara.CodeGen.Ir.IR;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Tests.IR;

public abstract class TestBase
{
    protected Module Module = null!;
    protected IrBuilder Builder = null!;

    [SetUp]
    public void Setup()
    {
        Module = new Module();
        var func = Module.AddFunction("test", new FunctionType());
        Builder = func.IrBuilder();
    }

    protected static void AssertIr(string actual, string expected)
    {
        var a = Trim(actual);
        var e = Trim(expected);
        Assert.That(a, Is.EqualTo(e));
    }

    private static string Trim(string str)
    {
        return string.Join('\n', str.Split('\n').Select(line => line.TrimStart())).Trim();
    }
}