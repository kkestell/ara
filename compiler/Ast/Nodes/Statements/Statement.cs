using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Statements;

public abstract record Statement(Node Node) : AstNode(Node);
