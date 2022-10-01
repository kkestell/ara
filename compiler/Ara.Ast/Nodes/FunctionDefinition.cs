using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, string Name, NodeList<Parameter> Parameters, TypeRef ReturnTypeRef, Block Block) : AstNode(Node), ITyped
{
    public override List<AstNode> Children { get; } = new() { ReturnTypeRef, Parameters, Block };
    
    public Type Type
    {
        get => ReturnTypeRef.ToType();
        set => throw new NotImplementedException();
    }
}
