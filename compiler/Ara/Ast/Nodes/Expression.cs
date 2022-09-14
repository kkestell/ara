using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record Expression(Node Node) : AstNode(Node)
{
    public InferredType? InferredType { get; set; }
}
