using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Block(IParseNode Node, NodeList<Statement> Statements) : AstNode(Node)
{
    readonly AstNode[] children = { Statements };

    public override IEnumerable<AstNode> Children => children;

    public Dictionary<string, ITyped> Scope { get; } = new();
}
