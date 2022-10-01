using Ara.Ast.Errors.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Errors;

public class SyntaxException : CompilerException
{
    public SyntaxException(Node node) : base(node, "Syntax error.")
    {
    }
}