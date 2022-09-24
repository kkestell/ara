using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record Definition(Node Node) : AstNode(Node);