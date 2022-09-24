using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, TypeRef ReturnType, string Name, NodeList<Parameter> Parameters, Block Block) : Definition(Node)
{
    public Type Type { get; } = new IntegerType();
    
    public override List<AstNode> Children { get; } = new List<AstNode> { ReturnType, Parameters, Block };
}
