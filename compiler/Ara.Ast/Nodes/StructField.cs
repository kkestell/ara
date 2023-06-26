using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record StructField(IParseNode Node, string Name, TypeRef TypeRef) : AstNode(Node), ITyped
{
    public override IEnumerable<AstNode> Children => new List<AstNode>();
    
    public Type Type => TypeRef.ToType();
}