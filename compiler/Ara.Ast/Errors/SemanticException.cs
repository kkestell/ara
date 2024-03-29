#region

using Ara.Ast.Errors.Abstract;
using Ara.Ast.Nodes.Abstract;

#endregion

namespace Ara.Ast.Errors;

public class SemanticException : CompilerException
{
    public SemanticException(AstNode astNode, string message) : base(astNode.Node, message)
    {
    }
}