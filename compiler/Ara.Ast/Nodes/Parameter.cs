#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes;

public record Parameter(IParseNode Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { TypeRef };

    public Type Type => TypeRef.ToType();
}
