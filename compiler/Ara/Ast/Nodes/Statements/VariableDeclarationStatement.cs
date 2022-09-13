using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclarationStatement(Node Node, Identifier Name, Expression Expression) : Statement(Node)
{
    public InferredType? InferredType { get; set; }
}
