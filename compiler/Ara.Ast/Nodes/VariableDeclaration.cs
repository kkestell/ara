using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, string Name, TypeRef? TypeRef, Expression? Expression) : Statement(Node), ITyped
{
    List<AstNode>? children;
    
    public Type Type { get; set; }

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
