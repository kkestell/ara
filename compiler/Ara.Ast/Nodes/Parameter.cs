using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record Parameter(IParseNode Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public Type Type => TypeRef.ToType();

    readonly AstNode[] children = { TypeRef };

    public override IEnumerable<AstNode> Children => children;
}
