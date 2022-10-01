using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class SemanticException : CompilerException
{
    public SemanticException(AstNode astNode, string message) : base(astNode.Node, message)
    {
    }
}