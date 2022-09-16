using Ara.Parsing;

namespace Ara.Tests;

public abstract class TestBase
{
    Parser parser;
    
    [SetUp]
    public void Setup()
    {
        parser = new Parser();
    }

    [TearDown]
    public void TearDown()
    {
        parser.Dispose();
    }

    protected Tree Parse(string src)
    {
        return parser.Parse(src);
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