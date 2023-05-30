#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes;

public record SingleValueTypeRef(IParseNode Node, string Name) : TypeRef(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode>();

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