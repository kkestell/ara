using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(IParseNode Node, string Name, NodeList<Parameter> Parameters, TypeRef ReturnTypeRef, Block Block) : AstNode(Node), ITyped
{
    readonly AstNode[] children = {  ReturnTypeRef, Parameters, Block  };

    public override IEnumerable<AstNode> Children => children;
    
    public Type Type
    {
        get => ReturnTypeRef.ToType();
        set => throw new NotImplementedException();
    }
}
