using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions;

public abstract record Expression(Node Node) : AstNode(Node), ITyped
{
    public abstract Type Type { get; set; }
}
