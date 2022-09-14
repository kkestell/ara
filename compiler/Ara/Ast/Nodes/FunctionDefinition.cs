using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, Identifier Name, List<Parameter> Parameters, Identifier ReturnType, Block Block) : Definition(Node)
{
    public readonly InferredType? InferredType = new (ReturnType.Value);
}
