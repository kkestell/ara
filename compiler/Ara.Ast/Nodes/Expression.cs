using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public abstract record Expression(Node Node) : AstNode(Node), ITyped
{
    public abstract Type Type { get; set; }
}
