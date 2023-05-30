#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Types;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions;

public record ArrayIndex(IParseNode Node, VariableReference VariableReference, Expression Index) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { VariableReference, Index };

    public override Type Type
    {
        get
        {
            if (VariableReference.Type is not ArrayType a)
                throw new SemanticException(this, $"Expected {VariableReference.Name} to be an array, not a {VariableReference.Type}");

            return a.Type;
        }
    }
}