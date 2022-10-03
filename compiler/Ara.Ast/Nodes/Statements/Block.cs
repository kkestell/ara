using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes.Statements;

public record Block(IParseNode Node, NodeList<Statement> Statements) : Statement(Node)
{
    readonly AstNode[] children = { Statements };

    public override IEnumerable<AstNode> Children => children;

    public Dictionary<string, ITyped> Scope { get; } = new();
}
