using Ara.Ast.Errors;
using Ara.Ast.Nodes.Expressions;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, string Name, TypeRef? TypeRef, Expression? Expression) : Statement(Node), ITyped
{
    List<AstNode>? children;

    public Type Type
    {
        get
        {
            if (TypeRef is not null)
            {
                return TypeRef.ToType();
            }

            if (Expression is not ITyped i)
                throw new SemanticException(this, "Cannot infer type of non-constant values");

            return i.Type;
        }

        set => throw new NotSupportedException();
    }

    public override List<AstNode> Children
    {
        get
        {
            if (children is not null)
                return children;

            children = new List<AstNode>();

            if (TypeRef is not null)
                children.Add(TypeRef);

            if (Expression is not null)
                children.Add(Expression);

            return children;
        }
    }
}
