using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record VariableDeclaration(Node Node, Identifier Name, Expression Expression) : Statement(Node)
{
    public InferredType? InferredType { get; set; }
}
