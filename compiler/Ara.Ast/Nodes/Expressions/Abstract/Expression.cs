using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Abstract;

public abstract record Expression(IParseNode Node) : AstNode(Node), ITyped
{
    public abstract Type Type { get; }
}
