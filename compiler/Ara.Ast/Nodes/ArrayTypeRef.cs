#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Types;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes;

public record ArrayTypeRef(IParseNode Node, TypeRef ElementTypeRef, IntegerValue Size) : TypeRef(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { ElementTypeRef, Size };
    
    public override Type ToType()
    {
        return new ArrayType(ElementTypeRef.ToType(), Size.Value);
    }
}