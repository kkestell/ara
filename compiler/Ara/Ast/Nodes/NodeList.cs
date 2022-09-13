using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record NodeList<T>(Node Node, IEnumerable<T> Nodes) : AstNode(Node) where T : AstNode;
