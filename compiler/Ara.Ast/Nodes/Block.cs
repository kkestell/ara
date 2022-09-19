using Ara.Parsing;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.Ast.Nodes;

public record Block(Node Node, IEnumerable<Statement> Statements) : AstNode(Node)
{
    public Dictionary<string, Type> Scope { get; } = new ();
}
