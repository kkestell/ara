using Ara.Parsing;

namespace Ara.Testing.Common;

public static class TestHelpers
{
    public static string Unindent(string input)
    {
        var lines = input.TrimStart('\n').Split("\n");

        if (lines.Length <= 0) return string.Join("\n", lines);
        
        var leadingWhitespaceCount = lines[0].TakeWhile(char.IsWhiteSpace).Count();

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length >= leadingWhitespaceCount && lines[i][..leadingWhitespaceCount].All(char.IsWhiteSpace))
            {
                lines[i] = lines[i][leadingWhitespaceCount..];
            }
            else
            {
                lines[i] = lines[i].TrimStart();
            }
        }

        return string.Join("\n", lines);
    }
    
    public static Parser CreateParser(string input)
    {
        var lexer = new Lexer(Unindent(input));
        return new Parser(lexer);
    }
}