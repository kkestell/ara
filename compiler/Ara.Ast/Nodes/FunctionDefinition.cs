using Ara.Ast.Semantics;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, TypeRef ReturnType, Identifier Name, List<Parameter> Parameters, Block Block) : Definition(Node)
{
    public readonly Type Type = new IntegerType();
}
