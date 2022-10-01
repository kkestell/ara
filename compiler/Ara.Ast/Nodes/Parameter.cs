using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public Type Type
    {
        get => TypeRef.ToType();
        set => throw new NotSupportedException();
    }

public override List<AstNode> Children { get; } = new() { TypeRef };
}
