using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class IfPredicateTypeException : CompilerException
{
    public IfPredicateTypeException(If astNode) : base(astNode.Node)
    {
        Description = $"Invalid predicate type {astNode.Predicate.InferredType!.Value} where bool was expected.";
    }
}