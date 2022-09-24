using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record Atom(Node Node) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { };
}