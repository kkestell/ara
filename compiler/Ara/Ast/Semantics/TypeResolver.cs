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
            case Constant c:
                ResolveConstant(c);
                break;
            case VariableReference v:
                ResolveVariableReference(v);
                break;
            case BinaryExpression b:
                ResolveBinaryExpression(b);
                break;
            case CallExpression c:
                ResolveCallExpression(c);
                break;
            case UnaryExpression u:
                ResolveUnaryExpression(u);
                break;
            case VariableDeclarationStatement d:
                ResolveVariableDeclarationStatement(d);
                break;
        }
    }

    static void ResolveConstant(Constant c)
    {
        c.InferredType = new InferredType(c.Type.Value);
    }

    static void ResolveBinaryExpression(BinaryExpression b)
    {
        if (b.Left.InferredType is null || b.Right.InferredType is null)
            throw new Exception("Cannot infer type of binary expression");

        if (!b.Left.InferredType.Equals(b.Right.InferredType))
            throw new BinaryExpressionTypeException(b);

        if (b.Op is BinaryOperator.Equality or BinaryOperator.Inequality)
        {
            b.InferredType = new InferredType("bool");
        }
        
        b.InferredType = b.Left.InferredType;
    }

    static void ResolveUnaryExpression(UnaryExpression u)
    {
        u.InferredType = u.Right.InferredType;
    }

    static void ResolveVariableReference(VariableReference e)
    {
        var type = e.ResolveVariableReference(e.Name.Value);

        if (type is null)
            throw new Exception($"Unable to find declaration of `{e.Name.Value}`.");

        e.InferredType = new InferredType(type);
    }

    static void ResolveCallExpression(CallExpression e)
    {
        var func = e.NearestAncestor<SourceFile>()!.Definitions.SingleOrDefault(x => x is FunctionDefinition d && d.Name.Value == e.Name.Value);

        if (func is not FunctionDefinition functionDefinition)
            throw new Exception($"Unable to find definition for function {e.Name.Value}");

        e.InferredType = new InferredType(functionDefinition.ReturnType.Value);
    }

    static void ResolveVariableDeclarationStatement(VariableDeclarationStatement d)
    {
        d.InferredType = d.Expression.InferredType;
    }
}
