using Ara.Parsing;

namespace Ara.Ast.Errors;

public class SyntaxException : CompilerException
{
    public SyntaxException(ParseNode parseNode) : base(parseNode, "Syntax error.")
    {
    }
}