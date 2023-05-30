#region

using Ara.Ast.Nodes.Expressions.Abstract;

#endregion

namespace Ara.Ast.Errors;

public class PredicateTypeException : SemanticException
{
    public PredicateTypeException(Expression node) : base(node, BuildMessage(node))
    {
    }

    private static string BuildMessage(Expression node)
    {
        return $"Invalid predicate type {node.Type} where bool was expected.";
    }
}