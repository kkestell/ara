using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclaration(IParseNode Node, string Name, TypeRef? TypeRef, Expression? Expression) : Statement(Node), ITyped
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

    public override IEnumerable<AstNode> Children
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
