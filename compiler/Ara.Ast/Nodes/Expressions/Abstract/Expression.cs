using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Abstract;

public abstract record Expression(Node Node) : AstNode(Node), ITyped
{
    public abstract Type Type { get; set; }
}
