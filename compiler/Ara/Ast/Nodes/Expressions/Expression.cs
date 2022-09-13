using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public abstract class Expression : AstNode
{
    protected Expression(Node node) : base(node)
    {
    }
    
    public InferredType? InferredType { get; set; }
}
