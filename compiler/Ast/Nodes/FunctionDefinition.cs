using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(Node Node, Identifier Name, List<Parameter> Parameters, TypeNode ReturnType, Block Block) : Definition(Node);
