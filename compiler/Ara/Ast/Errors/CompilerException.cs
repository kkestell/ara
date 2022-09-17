using System.Text;
using Ara.Parsing;

namespace Ara.Ast.Errors;

public abstract class CompilerException : Exception
{
    protected CompilerException(Node node)
    {
        Node = node;
    }

    protected Node Node { get; }

    protected Location Location => Node.Location;
    
    protected string Indented(string str)
    {
        return Indent(Location.Column, $"↑ {str}");
    }

    static string Indent(int col, string str)
    {
        return $"{new string(' ', col - 1)}{str}";
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine(Location.ToString());
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented($"Something went wrong here"));
        
        return sb.ToString();
    }
}