using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, string Name, NodeList<Parameter> Parameters, TypeRef ReturnType, Block Block) : AstNode(Node), ITyped
{
    public override List<AstNode> Children { get; } = new List<AstNode> { ReturnType, Parameters, Block };
    
    public Type Type
    {
        get => Type.Parse(ReturnType);
        set => throw new NotImplementedException();
    }
}
