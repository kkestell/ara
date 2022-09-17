using Ara.Parsing;

namespace Ara.Ast.Errors;

public class GenericCompilerException : CompilerException
{
    public GenericCompilerException(Node node) : base(node)
    {
    }
}