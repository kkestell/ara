using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, Identifier Name, List<Parameter> Parameters, Type_ ReturnType, Block Block) : Definition(Node);
