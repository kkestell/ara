using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Block(Node Node, IEnumerable<Statement> Statements) : AstNode(Node)
{
    public Dictionary<string, InferredType> Scope { get; } = new ();
}
