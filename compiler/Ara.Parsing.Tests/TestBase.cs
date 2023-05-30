#region

using System.Text.RegularExpressions;

#endregion

namespace Ara.Parsing.Tests;

public abstract class TestBase
{
    protected Parser Parser;
    
    [SetUp]
    public void Setup()
    {
        Parser = new Parser();
    }

    [TearDown]
    public void TearDown()
    {
        Parser.Dispose();
    }
    
    protected static void AssertSexp(string actual, string expected)
    {
        var a = Trim(actual);
        var e = Trim(expected);
        Assert.That(a, Is.EqualTo(e));
    }

    private static string Trim(string str)
    {
        return Regex.Replace(str, "[ \r\n]*", "");
    }
}