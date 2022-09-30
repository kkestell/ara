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
            //case VariableReference v:
            //    ResolveVariableReference(v);
            //    break;
            //case BinaryExpression b:
            //    ResolveBinaryExpression(b);
            //    break;
            //case UnaryExpression u:
            //    ResolveUnaryExpression(u);
            //    break;
            case Call c:
                ResolveCallExpression(c);
                break;
            case VariableDeclaration v:
                ResolveVariableDeclaration(v);
                break;
        }
    }

    /*
    static void ResolveVariableReference(VariableReference r)
    {
        var node = r.ResolveReference(r.Name);

        if (node is null)
            throw new ReferenceException(r);

        var type = node switch
        {
            VariableDeclaration d => d.Type,
            Parameter           p => p.Type,
            For                   => new IntegerType(),

            _ => throw new Exception($"Unsupported node {node.GetType()}")
        };

        r.Type = type;
    }
    */
    
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
        var func = c.NearestAncestor<SourceFile>().FunctionDefinitions.Nodes.SingleOrDefault(x => x.Name == c.Name);

        if (func is null)
            throw new ReferenceException(c);

        c.Type = Type.Parse(func.ReturnType);
    }
    
    static void ResolveVariableDeclaration(VariableDeclaration v)
    {
        if (v.TypeRef is not null)
        {
            v.Type = Type.Parse(v.TypeRef);
            return;
        }

        if (v.Expression is not ITyped i)
            throw new SemanticException(v, "Cannot infer type of non-constant values");

        v.Type = i.Type;
    }
}
