using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record Statement(Node Node) : AstNode(Node);
