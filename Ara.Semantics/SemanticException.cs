using System.Text;
using Ara.Parsing;

namespace Ara.Semantics;

public class SemanticException : Exception
{
    public Location Location { get; }
    public string? FileName { get; }
    public string RawMessage { get; }
    
    public SemanticException(string message, Location location, string? fileName) : base(FormatMessage(message, location, fileName))
    {
        Location = location;
        FileName = fileName;
        RawMessage = message;
    }

    private static string FormatMessage(string message, Location location, string? fileName)
    {
        var sb = new StringBuilder();
        var marker = new string('-', (int)location.Column - 1) + '^';

        sb.Append($"{location.LineContent}\n{marker}\n");
        
        if (!string.IsNullOrEmpty(fileName))
            sb.Append($"{fileName} ");
        
        sb.Append($"({location.Row},{location.Column}) {message}");
        
        return sb.ToString();
    }
}