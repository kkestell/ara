using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Call(Node Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Arguments };
}
