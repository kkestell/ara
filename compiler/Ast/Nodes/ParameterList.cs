using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record ParameterList(Node Node, IEnumerable<Parameter> Values) : AstNode(Node);
