using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Types;

namespace Ara.Ast.Semantics;

public class TypeChecker : Visitor
{
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Return r:
                CheckReturnStatement(r);
                break;

            case If i:
                CheckIfStatement(i);
                break;
        }
    }

    void CheckReturnStatement(Return r)
    {
        var func = r.NearestAncestor<FunctionDefinition>();

        if (func is null)
            throw new Exception("This shouldn't be possible.");
            
        if (r.Expression.InferredType is null)
            throw new Exception("Unable to infer type of return expression!");
            
        if (!r.Expression.InferredType.Equals(func.InferredType))
            throw new ReturnTypeException(r);
    }

    void CheckIfStatement(If i)
    {
        if (i.Predicate.InferredType is null)
            throw new Exception("Unable to infer predicate type");
                
        if (!i.Predicate.InferredType.Equals(new InferredType("bool")))
            throw new IfPredicateTypeException(i);
    }
}