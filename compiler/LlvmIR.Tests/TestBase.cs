global using System;
global using System.Collections.Generic;
global using System.Linq;
global using NUnit.Framework;
using Ara.CodeGen;
using Ara.CodeGen.Types;

namespace LlvmIR.Tests;

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