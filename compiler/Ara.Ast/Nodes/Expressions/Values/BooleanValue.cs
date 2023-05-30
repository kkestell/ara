#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions.Values;

public record BooleanValue(IParseNode Node, bool Value) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode>();

    public override Type Type => Type.Boolean;
}