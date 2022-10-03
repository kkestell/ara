using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record SingleValueTypeRef(IParseNode Node, string Name) : TypeRef(Node)
{
    readonly AstNode[] children = {};

    public override IEnumerable<AstNode> Children => children;
    
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