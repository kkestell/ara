using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record Block(Node Node, NodeList<Statement> Statements) : AstNode(Node)
{
    public Dictionary<string, Type> Scope { get; } = new ();
    
    public override List<AstNode> Children { get; } = new List<AstNode> { Statements };
}
