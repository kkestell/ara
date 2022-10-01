using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Block(Node Node, NodeList<Statement> Statements) : AstNode(Node)
{
    public Dictionary<string, ITyped> Scope { get; } = new();
    
    public override List<AstNode> Children { get; } = new() { Statements };
}
