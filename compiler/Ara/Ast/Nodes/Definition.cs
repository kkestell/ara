using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract class Definition : AstNode
{
    protected Definition(Node node) : base(node)
    {
    }
}
