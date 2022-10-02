using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Values;

public record BooleanValue(Node Node, bool Value) : Expression(Node)
{
    public override List<AstNode> Children => new();

    public override Type Type
    {
        get => Type.Boolean;
        set => throw new NotImplementedException();
    }
}