using Ara.Parsing;

namespace Ara.Ast.Errors;

public class GenericCompilerException : CompilerException
{
    public GenericCompilerException(Node node, string description) : base(node)
    {
        Description = description;
    }
}