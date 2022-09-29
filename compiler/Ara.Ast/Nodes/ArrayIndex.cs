using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ArrayIndex(Node Node, VariableReference VariableReference, Expression Index) : Atom(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { VariableReference, Index };
}