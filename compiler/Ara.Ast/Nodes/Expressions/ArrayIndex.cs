using Ara.Ast.Errors;
using Ara.Ast.Types;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions;

public record ArrayIndex(Node Node, VariableReference VariableReference, Expression Index) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new() { VariableReference, Index };

    public override Type Type
    {
        get
        {
            if (VariableReference.Type is not ArrayType a)
                throw new SemanticException(this, $"Expected {VariableReference.Name} to be an array, not a {VariableReference.Type}");

            return a.Type;
        }
        
        set => throw new NotImplementedException();
    }
}