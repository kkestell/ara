using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions.Values;

public record IntegerValue(IParseNode Node, int Value) : Expression(Node)
{
    public override Type Type
    {
        get => Type.Integer;
        set => throw new NotImplementedException();
    }
    
    readonly AstNode[] children = {};

    public override IEnumerable<AstNode> Children => children;
}