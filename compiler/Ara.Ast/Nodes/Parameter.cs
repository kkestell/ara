using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record Parameter(Node Node, string Name, TypeRef TypeRef) : AstNode(Node)
{
    public Type Type { get; } = Type.Parse(TypeRef);

    public override List<AstNode> Children { get; } = new List<AstNode> { TypeRef };
}
