using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Types;

namespace Ara.Ast.Semantics;

public class TypeResolver : Visitor
{
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Integer i:
                ResolveInt(i);
                break;
            
            case Float f:
                ResolveFloat(f);
                break;
            
            case String_ s:
                ResolveString(s);
                break;
            
            case Bool b:
                ResolveBool(b);
                break;
            
            case BinaryExpression b:
                ResolveBinaryExpression(b);
                break;

            case UnaryExpression u:
                ResolveUnaryExpression(u);
                break;
            
            case VariableReference v:
                ResolveVariableReference(v);
                break;
            
            case FunctionCallExpression:
                throw new NotImplementedException();
        }
    }

    static void ResolveBool(Expression e)
    {
        e.InferredType = new InferredType("bool");
    }
    
    static void ResolveFloat(Expression e)
    {
        e.InferredType = new InferredType("float");
    }
    
    static void ResolveInt(Expression e)
    {
        e.InferredType = new InferredType("int");
    }

    static void ResolveString(Expression e)
    {
        e.InferredType = new InferredType("string");
    }

    static void ResolveBinaryExpression(BinaryExpression b)
    {
        if (b.Left.InferredType is null || b.Right.InferredType is null)
            throw new Exception("Cannot infer type of binary expression");

        if (!b.Left.InferredType.Equals(b.Right.InferredType))
            throw new BinaryExpressionTypeException(b);

        b.InferredType = b switch
        {
            LogicalComparisonExpression => new InferredType("bool"),
            ArithmeticExpression        => b.Left.InferredType,
            _ => throw new NotImplementedException()
        };
    }

    static void ResolveUnaryExpression(UnaryExpression u)
    {
        u.InferredType = u.Right.InferredType;
    }

    static void ResolveVariableReference(VariableReference v)
    {
        var blk = v.NearestAncestor<Block>();

        while (true)
        {
            if (blk is null)
                break;

            var decl = blk.Statements.SingleOrDefault(s => s is VariableDeclarationStatement d && d.Name.Value == v.Name.Value);

            if (decl is VariableDeclarationStatement d)
            {
                v.InferredType = new InferredType(d.Type.Value);
                return;
            }

            blk = blk.NearestAncestor<Block>();
        }

        throw new Exception($"Unable to find declaration of `{v.Name}`.");
    }
}
