using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Type_(Node Node, string Value) : AstNode(Node);
