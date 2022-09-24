using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Semantics.Types;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Semantics;

public class TypeResolver : Visitor
{
    public TypeResolver(SourceFile sourceFile) : base(sourceFile)
    {
    }
    
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
        var type = r.ResolveType(r.Name);

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

        b.Type = b.Op is BinaryOperator.Equality or BinaryOperator.Inequality ? new BooleanType() : b.Left.Type;        
    }

    static void ResolveUnaryExpression(UnaryExpression u)
    {
        u.Type = u.Right.Type;
    }

    static void ResolveCallExpression(Call c)
    {
        var func = c.NearestAncestor<SourceFile>()
            .Definitions.Nodes.SingleOrDefault(x => x is FunctionDefinition d && d.Name == c.Name);

        if (func is not FunctionDefinition functionDefinition)
            throw new ReferenceException(c);

        c.Type = Type.Parse(functionDefinition.ReturnType);
    }
}
