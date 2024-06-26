using System.Text;

namespace Ara.Parsing;

public class ParsingException : Exception
{
    public Location Location { get; }

    public ParsingException(string message, Location location) : base(FormatMessage(message, location))
    {
        Location = location;
    }

    private static string FormatMessage(string message, Location location)
    {
        var sb = new StringBuilder();
        var marker = new string('-', (int)location.Column - 1) + '^';

        sb.Append($"{location.LineContent}\n{marker}\n");
        
        if (!string.IsNullOrEmpty(location.FileName))
            sb.Append($"{location.FileName} ");
        
        sb.Append($"({location.Row},{location.Column}) {message}");
        
        return sb.ToString();
    }
}
