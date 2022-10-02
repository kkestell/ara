using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Values;

public record FloatValue(Node Node, float Value) : Expression(Node)
{
    public override Type Type
    {
        get => Type.Float;
        set => throw new NotImplementedException();
    }

    public override List<AstNode> Children => new();
}