using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Types;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public record ArrayTypeRef(Node Node, TypeRef ElementTypeRef, IntegerValue Size) : TypeRef(Node)
{
    public override List<AstNode> Children { get; } = new() { ElementTypeRef, Size };
    
    public override Type ToType()
    {
        return new ArrayType(ElementTypeRef.ToType(), Size.Value);
    }
}