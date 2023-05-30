#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes.Statements;

public record Block(IParseNode Node, NodeList<Statement> Statements) : Statement(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Statements };

    public Dictionary<string, ITyped> Scope { get; } = new();
}
