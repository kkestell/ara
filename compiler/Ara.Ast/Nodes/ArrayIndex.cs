using Ara.Ast.Errors;
using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record ArrayIndex(Node Node, VariableReference VariableReference, Expression Index) : Atom(Node)
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