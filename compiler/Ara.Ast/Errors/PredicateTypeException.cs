using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;

namespace Ara.Ast.Errors;

public class PredicateTypeException : SemanticException
{
    public PredicateTypeException(Expression node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(Expression node)
    {
        return $"Invalid predicate type {node.Type} where bool was expected.";
    }
}