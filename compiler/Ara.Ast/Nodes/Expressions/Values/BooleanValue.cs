using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Values;

public record BooleanValue(IParseNode Node, bool Value) : Expression(Node)
{
    readonly AstNode[] children = { };

    public override IEnumerable<AstNode> Children => children;

    public override Type Type
    {
        get => Type.Boolean;
        set => throw new NotImplementedException();
    }
}