using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public abstract record TypeRef(Node Node) : AstNode(Node)
{
    public abstract Type ToType();
}
