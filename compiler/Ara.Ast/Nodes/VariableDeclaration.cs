using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, TypeRef TypeRef, string Name, Expression? Expression) : Statement(Node)
{
    List<AstNode>? children;
    
    public readonly Type Type = Type.Parse(TypeRef);

    public override List<AstNode> Children
    {
        get
        {
            if (children is not null)
                return children;
            
            children = new List<AstNode> { TypeRef };

            if (Expression is not null)
                children.Add(Expression);

            return children;
        }
    }
}
