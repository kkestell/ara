using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record SingleValueTypeRef(Node Node, string Name) : TypeRef(Node)
{
    public override List<AstNode> Children { get; } = new();
    
    public override Type ToType()
    {
        return Name switch
        {
            "bool"  => Type.Boolean,
            "float" => Type.Float,
            "int"   => Type.Integer,
            "void"  => Type.Void,
            
            _ => throw new Exception()
        };
    }
}