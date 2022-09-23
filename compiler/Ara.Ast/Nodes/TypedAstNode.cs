using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public abstract record TypedAstNode(Node Node) : AstNode(Node)
{
    public abstract Type Type { get; set; }
}