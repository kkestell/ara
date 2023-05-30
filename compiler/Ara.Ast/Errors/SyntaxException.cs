#region

using Ara.Ast.Errors.Abstract;
using Ara.Parsing;

#endregion

namespace Ara.Ast.Errors;

public class SyntaxException : CompilerException
{
    public SyntaxException(ParseNode parseNode) : base(parseNode, "Syntax error.")
    {
    }
}