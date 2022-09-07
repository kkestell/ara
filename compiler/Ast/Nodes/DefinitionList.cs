using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record DefinitionList(Node Node, IEnumerable<Definition> Values) : AstNode(Node);