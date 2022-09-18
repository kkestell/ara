using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class IfPredicateTypeException : SemanticException
{
    IfPredicateTypeException(If node, string message) : base(node.Node, message)
    {
    }

    public static IfPredicateTypeException Create(If node)
    {
        var message = $"Invalid predicate type {node.Predicate.InferredType!.Value} where bool was expected.";
        return new IfPredicateTypeException(node, message);
    }
}