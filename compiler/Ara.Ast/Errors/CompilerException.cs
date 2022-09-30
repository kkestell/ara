using System.Text;
using Ara.Ast.Nodes;
using Ara.Parsing;

namespace Ara.Ast.Errors;

public class CompilerException : Exception
{
    public CompilerException(Node node, string message) : base(message)
    {
        Node = node;
    }

    public Node Node { get; }

    public Location Location => Node.Location;

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Error in {Location.ToString()}");
        sb.AppendLine();
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented("^~~~"));
        sb.AppendLine();
        sb.AppendLine(Message);
        
        return sb.ToString();
    }
    
    string Indented(string str)
    {
        return Indent(Location.Column, str);
    }
    
    static string Indent(int col, string str)
    {
        return $"{new string(' ', col)}{str}";
    }
}