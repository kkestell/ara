#region

using System.Text;
using Ara.Parsing;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Errors.Abstract;

public abstract class CompilerException : Exception
{
    protected CompilerException(IParseNode parseNode, string message) : base(message)
    {
        ParseNode = parseNode;
    }

    private IParseNode ParseNode { get; }

    private Location Location => ParseNode.Location;

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

    private string Indented(string str)
    {
        return Indent(Location.Column, str);
    }

    private static string Indent(int col, string str)
    {
        return $"{new string(' ', col)}{str}";
    }
}