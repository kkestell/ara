using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record Parameter(IParseNode Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public Type Type
    {
        get => TypeRef.ToType();
        set => throw new NotSupportedException();
    }

public override List<AstNode> Children { get; } = new() { TypeRef };
}
