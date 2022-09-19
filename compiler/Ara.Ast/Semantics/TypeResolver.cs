using Ara.Ast.Errors;
using Ara.Ast.Nodes;

namespace Ara.Ast.Semantics;

public class TypeResolver : Visitor
{
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case VariableReference v:
                ResolveVariableReference(v);
                break;
            case BinaryExpression b:
                ResolveBinaryExpression(b);
                break;
            case UnaryExpression u:
                ResolveUnaryExpression(u);
                break;
            case Call c:
                ResolveCallExpression(c);
                break;
        }
    }

    static void ResolveVariableReference(VariableReference r)
    {
        var type = r.ResolveVariableReference(r.Name.Value);

        if (type is null)
            throw new ReferenceException(r);

        r.Type = type;
    }
    
    static void ResolveBinaryExpression(BinaryExpression b)
    {
        if (b.Left.Type is null || b.Right.Type is null)
            throw new BinaryExpressionTypeException(b);

        if (!b.Left.Type.Equals(b.Right.Type))
            throw new BinaryExpressionTypeException(b);

        if (b.Op is BinaryOperator.Equality or BinaryOperator.Inequality)
        {
            b.Type = new BooleanType();
        }
        else
        {
            b.Type = b.Left.Type;
        }        
    }

    static void ResolveUnaryExpression(UnaryExpression u)
    {
        u.Type = u.Right.Type;
    }

    static void ResolveCallExpression(Call c)
    {
        var func = c.NearestAncestor<SourceFile>()!
            .Definitions.SingleOrDefault(x => x is FunctionDefinition d && d.Name.Value == c.Name.Value);

        if (func is not FunctionDefinition functionDefinition)
            throw new ReferenceException(c);

        c.Type = Type.Parse(functionDefinition.ReturnType.Value);
    }
}
