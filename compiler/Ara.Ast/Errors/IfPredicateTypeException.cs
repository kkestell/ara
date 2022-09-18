using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class IfPredicateTypeException : SemanticException
{
    public IfPredicateTypeException(If node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(If node)
    {
        return $"Invalid predicate type {node.Predicate.InferredType!.Value} where bool was expected.";
    }
}