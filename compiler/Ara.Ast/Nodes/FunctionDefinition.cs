using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, Identifier ReturnType, Identifier Name, List<Parameter> Parameters, Block Block) : Definition(Node)
{
    public readonly InferredType? InferredType = new (ReturnType.Value);
}