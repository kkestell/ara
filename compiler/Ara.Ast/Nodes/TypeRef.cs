using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record TypeRef(Node Node) : AstNode(Node);