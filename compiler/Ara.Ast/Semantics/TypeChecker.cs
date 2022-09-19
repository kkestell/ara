using Ara.Ast.Errors;
using Ara.Ast.Nodes;

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
        
        if (r.Expression.Type is null)
            throw new SemanticException(r.Expression, "Expression type could not be inferred.");
            
        if (!r.Expression.Type.Equals(func.Type))
            throw new ReturnTypeException(r);
    }

    void CheckIfStatement(If i)
    {
        if (i.Predicate.Type is null)
            throw new SemanticException(i.Predicate, "Expression type could not be inferred.");
                
        if (!i.Predicate.Type.Equals(new BooleanType()))
            throw new IfPredicateTypeException(i);
    }
}