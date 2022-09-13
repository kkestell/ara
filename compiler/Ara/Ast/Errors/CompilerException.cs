using Ara.Parsing;

namespace Ara.Ast.Errors;

public abstract class CompilerException : Exception
{
    protected CompilerException(Node node)
    {
        Node = node;
    }

    protected Node Node { get; }
}