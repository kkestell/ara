using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public Type Type
    {
        get => Type. Parse(TypeRef);
        set => throw new NotSupportedException();
    }

public override List<AstNode> Children { get; } = new List<AstNode> { TypeRef };
}
