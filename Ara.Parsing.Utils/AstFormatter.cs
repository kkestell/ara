using System.Xml.Linq;

namespace Ara.Parsing.Utils;

public class AstFormatter
{
    public static string Format(string input)
    {
        var document = XDocument.Parse(input);
        return document.ToString();
    }
}