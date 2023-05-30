#region

using Ara.Parsing;

#endregion

namespace Ara.Ast.Tests;

public abstract class TestBase
{
    private Parser? _parser;
    
    [SetUp]
    public void Setup()
    {
        _parser = new Parser();
    }

    [TearDown]
    public void TearDown()
    {
        _parser?.Dispose();
    }

    protected Tree Parse(string src)
    {
        if (_parser is null)
            throw new InvalidOperationException("Parser is null");
        
        return _parser.Parse(src);
    }
    
    // protected static void AssertIr(string actual, string expected)
    // {
    //     var a = Trim(actual);
    //     var e = Trim(expected);
    //     Assert.That(a, Is.EqualTo(e));
    // }

    // private static string Trim(string str)
    // {
    //     return string.Join('\n', str.Split('\n').Select(line => line.TrimStart())).Trim();
    // }
}