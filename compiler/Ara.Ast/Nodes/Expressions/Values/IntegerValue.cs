using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions.Values;

public record IntegerValue(Node Node, int Value) : Expression(Node)
{
    public override Type Type
    {
        get => Type.Integer;
        set => throw new NotImplementedException();
    }
    
    public override List<AstNode> Children => new();
}