using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, TypeRef ReturnType, string Name, List<Parameter> Parameters, Block Block) : Definition(Node)
{
    public readonly Type Type = new IntegerType();
}
