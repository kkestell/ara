using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class PredicateTypeException : SemanticException
{
    public PredicateTypeException(If node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(If node)
    {
        return $"Invalid predicate type {node.Predicate.Type} where bool was expected.";
    }
}