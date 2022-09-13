using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public abstract class Statement : AstNode
{
    public Statement(Node node) : base(node)
    {
    }
}
